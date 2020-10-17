import { Component, OnInit, ViewChild } from '@angular/core';
import { SiteConstants } from '@constants/siteConstants';
import { formatBooleanToStatus } from '@shared/components/grid/formatters/formatBooleanToStatus';
import { GenericGridComponent } from '@shared/components/grid/generic-grid/generic-grid.component';
import { AppSettingsService } from '@shared/services/app-settings.service';
import { HttpService } from '@shared/services/http-service.service';
import { ColDef, Environment, GridOptions } from 'ag-grid-community';
import { environment } from 'src/environments/environment';
import { ServiceStatus } from './serviceStatus';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  @ViewChild(GenericGridComponent, { static: false }) grid;
  serviceList: Array<ServiceStatus>;

  columnDefs: Array<ColDef>;
  GridCssStyle: string;
  gridOptions: GridOptions;
  interval: any;
  waitInterval: number = 5000;

  constructor(private _httpClient: HttpService, private _appSettingsService: AppSettingsService) {
    this.columnDefs = this.createColumnDefs();
    this.GridCssStyle = "width: 100%; height: 450px;";
    this.gridOptions = <GridOptions>{};
  }

  ngOnInit() {

    // get the list of servers from the config file
    var serviceStatus = this._appSettingsService.GetValue(SiteConstants.serviceStatus);
    // empty the service list
    this.serviceList = [];

    // pre-populate the list with all the services to check
    serviceStatus.Services.forEach(async (service) => {
      this.serviceList.push({ name: service.Name, isRunning: true, url: service.Uri });
    });

    // Do the intial check of the services to populate the correct values in the grid
    this.CheckServices();

    // start the timer process to check the services
    this.startTimer();
  }

  CheckServices() {
    // get the list of servers from the config file
    var serviceStatus = this._appSettingsService.GetValue(SiteConstants.serviceStatus);

    // iterate through the list and check each service
    serviceStatus.Services.forEach((service) => {

      // call the service but get back the response object not the json retured from the health check
      this._httpClient.getDataAndResponse(service.Uri).then(response => {

        // make sure you have a valid response
        if (response) {

          // if the status is 200 then set the value to running
          if (response.status === 200) {
            var pos = this.serviceList.find(exp => exp.url.toUpperCase() === response.url.toUpperCase());
            pos.isRunning = true;
          }
          else {
            // Healthcheck returned someting wrong, flag service as DOWN
            var pos = this.serviceList.find(exp => exp.url.toUpperCase() === response.url.toUpperCase());
            pos.isRunning = false;
          }


        } else { // Healthcheck returned bad response object, flag service as DOWN
          var pos = this.serviceList.find(exp => exp.url.toUpperCase() === service.Uri.toUpperCase());
          pos.isRunning = false;
        }

        // refresh grid with status
        this.grid.refresh();
      });

    });

  }

  startTimer() {
    this.interval = setInterval(() => {
      this.CheckServices();
    }, this.waitInterval)
  }

  stopTimer() {
    clearInterval(this.interval);
  }

  createColumnDefs() {
    return [
      {
        headerName: "Service",
        field: "name",
        width: 200

      },
      {
        headerName: "Status",
        field: "isRunning",
        cellClass: cellClass,
        width: 50,
        valueFormatter: formatBooleanToStatus
      }
    ];
  }
}

// color the running column based on the services status
function cellClass(params) {
  return params.value ? 'rag-green' : 'rag-red';
}
