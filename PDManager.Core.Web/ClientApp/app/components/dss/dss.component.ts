import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'dss',
    templateUrl: './dss.component.html'
})
export class DSSComponent {
    public dssModels: DSSModel[];
    public logOutput: LogOutput[];

    private httpHandler: Http;
    private dssFetchUrl: string;
    private dssDummyUrl: string;


    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.httpHandler = http;
        this.dssFetchUrl = baseUrl + 'api/v1/dss';
        this.dssDummyUrl = baseUrl + 'api/v1/dss/dummy';        
        this.logOutput = [];
        this.httpHandler.get(this.dssFetchUrl).subscribe(result => {
            this.dssModels = result.json() as DSSModel[];
            this.logOutput.push({
                message: 'DSS Models loaded', color: "#982315", error: false
            });
        }, error => console.error(error));
    }
    public refresh(): void {

        this.httpHandler.get(this.dssFetchUrl).subscribe(result => {
            this.dssModels = result.json() as DSSModel[];
            this.logOutput.push({
                message: 'DSS Models loaded', color: "#982315", error: false});
        }, error => console.error(error));

    };
  
    public addDummyData():void {

        this.httpHandler.get(this.dssDummyUrl).subscribe(result => {
            this.refresh();
        }, error => console.error(error));

    };
}

interface LogOutput {

    message: string;
    error: boolean;
    color: string;
    

}
interface DSSModel {
    id: number;
    title: string;
    description: string;
    createdBy: string;
    execute(): void;
    
}
