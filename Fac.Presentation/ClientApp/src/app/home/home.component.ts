import { Component, OnInit } from "@angular/core";
import { environment } from "@environment/environment";
import { HttpService } from "@shared/services/http-service.service";
import { SignalrHubService } from "@shared/services/signalr-hub.service";
import { MessageService } from "primeng";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent implements OnInit {
  // if you need multiple connections then you have to create the service with new and not inject
  constructor(
    private _mibConnection: SignalrHubService,
    private _facDecisionConnection: SignalrHubService,
    private _facCaseConnection: SignalrHubService,
    private messageService: MessageService,
    private _httpService: HttpService
  ) {}

  async ngOnInit() {
    await this.CreateMibConnection();

    // this._facDecisionConnection.URL = `${environment.apiGatewayUrl}/facdecision`
    // this._facCaseConnection.URL = `${environment.apiGatewayUrl}/faccase`
  }

  async CreateMibConnection() {
    this._mibConnection.URL = `${environment.apiGatewayUrl}/mibhub`


    await this._mibConnection.Connect();

    if (this._mibConnection.IsConnected) {
      this.RegisterMibEvents();
    }

  }
  protected RegisterMibEvents(): void {
    this._mibConnection.Hub.on("MibCompleted", (data: any) => {
      console.log(data);
      const received = `MIB Completed`;
      this.messageService.add({
        severity: "info",
        detail: received,
        life: 3000,
      });
    });

    // FacDecisionMade for facdecision hub
    // FacCaseSubmitted fpr fac case hub
  }

  submitMib() {
    this._httpService.getData(`${environment.apiGatewayUrl}/fac/submitMib`);
    this.messageService.add({
      severity: "info",
      summary: "Mib Submitted",
      life: 2000,
    });
  }
}
