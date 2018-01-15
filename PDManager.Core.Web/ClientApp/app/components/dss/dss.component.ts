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
    private dssSchemaUrl: string;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.httpHandler = http;
        this.dssFetchUrl = baseUrl + 'api/v1/dss';
        this.dssDummyUrl = baseUrl + 'api/v1/dss/dummy';        
        this.dssSchemaUrl = baseUrl + 'api/v1/dss/schema';        
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
    public getSchema(): void {

        this.httpHandler.get(this.dssSchemaUrl).subscribe(result => {
            var schema = result.json() as string;
            this.logOutput.push({
                message: schema, color: "#982315", error: false
            });
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
