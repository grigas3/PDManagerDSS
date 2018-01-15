import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'alert',
    templateUrl: './alert.component.html'
})
export class AlertComponent {
    public alertModels: AlertModel[];
    public logOutput: LogOutput[];

    private httpHandler: Http;
    private alertFetchUrl: string;
    private alertDummyUrl: string;


    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.httpHandler = http;
        this.alertFetchUrl = baseUrl + 'api/v1/alert';
        this.alertDummyUrl = baseUrl + 'api/v1/alert/dummy';        
        this.logOutput = [];
        this.httpHandler.get(this.alertFetchUrl).subscribe(result => {
            this.alertModels = result.json() as AlertModel[];
            this.logOutput.push({
                message: 'Alert Models loaded', color: "#982315", error: false
            });
        }, error => console.error(error));
    }
    public refresh(): void {

        this.httpHandler.get(this.alertFetchUrl).subscribe(result => {
            this.alertModels = result.json() as AlertModel[];
            this.logOutput.push({
                message: 'Alert Models loaded', color: "#982315", error: false});
        }, error => console.error(error));

    };
  
    public addDummyData():void {

        this.httpHandler.get(this.alertDummyUrl).subscribe(result => {
            this.refresh();
        }, error => console.error(error));

    };

    public execute(id: string): void {

        alert(id);
    }
}

interface LogOutput {

    message: string;
    error: boolean;
    color: string;
    

}
interface AlertModel {
    id: number;
    title: string;
    description: string;
    createdBy: string;
    
    
}
