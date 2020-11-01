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
  private _mibConnection: SignalrHubService;
  private _facDecisionConnection: SignalrHubService;
  private _facCaseConnection: SignalrHubService;

  constructor( private messageService: MessageService, private _httpService: HttpService) {
    // cant inject SignalrHubSerivce if you have multiple listeners becuase the service is a singleton
    this._mibConnection = new SignalrHubService();
    this._facDecisionConnection = new SignalrHubService();
    this._facCaseConnection = new SignalrHubService();
  }

  async ngOnInit() {
    await this.CreateMibConnection();
    await this.CreateFacDecisionConnection();
    await this.CreateFacSumbittedConnection();
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
  }

  async CreateFacDecisionConnection() {
     this._facDecisionConnection.URL = `${environment.apiGatewayUrl}/facdecision`
    await this._facDecisionConnection.Connect();

    if (this._facDecisionConnection.IsConnected) {
      this.RegisterFacDecisionEvents();
    }
  }

  protected RegisterFacDecisionEvents(): void {
    this._facDecisionConnection.Hub.on("FacDecisionMade", (data: any) => {
      console.log(data);
      const received = `Fac Decision Made`;
      this.messageService.add({
        severity: "info",
        detail: received,
        life: 3000,
      });
    });
  }

  async CreateFacSumbittedConnection() {
    this._facCaseConnection.URL = `${environment.apiGatewayUrl}/faccase`

   await this._facCaseConnection.Connect();

   if (this._facCaseConnection.IsConnected) {
     this.RegisterFacSubmittedEvents();
   }
 }

 protected RegisterFacSubmittedEvents(): void {
  this._facCaseConnection.Hub.on("FacCaseSubmitted", (data: any) => {
    console.log(data);
    const received = `Fac Case Submitted`;
    this.messageService.add({
      severity: "info",
      detail: received,
      life: 3000,
    });
  });
}


  submitMib() {
    this._httpService.getData(`${environment.apiGatewayUrl}/fac/submitMib`);
    this.messageService.add({
      severity: "info",
      summary: "Mib Submitted",
      life: 2000,
    });
  }

  submitCaseDecision() {
    this._httpService.getData(`${environment.apiGatewayUrl}/fac/FacCaseDecision`);
    this.messageService.add({
      severity: "info",
      summary: "Case Decsion Submitted",
      life: 2000,
    });
  }

  submitCase() {
    this._httpService.getData(`${environment.apiGatewayUrl}/fac/SubmitCase`);
    this.messageService.add({
      severity: "info",
      summary: "Case Submitted",
      life: 2000,
    });
  }
}
