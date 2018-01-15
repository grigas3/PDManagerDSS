import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'aggregation',
    templateUrl: './aggr.component.html'
})
export class AggregationComponent {
    public aggrModels: AggrModel[];
    public logOutput: LogOutput[];    
    private httpHandler: Http;
    private dssFetchUrl: string;
    private dssDummyUrl: string;
    private dssSchemaUrl: string;
    private dssExecuteUrl: string;
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.httpHandler = http;
        this.dssFetchUrl = baseUrl + 'api/v1/aggregation';
        this.dssDummyUrl = baseUrl + 'api/v1/aggregation/dummy';        
        this.dssSchemaUrl = baseUrl + 'api/v1/aggregation/schema';        
        this.logOutput = [];
        this.httpHandler.get(this.dssFetchUrl).subscribe(result => {
            this.aggrModels = result.json() as AggrModel[];
            this.logOutput.push({
                message: 'Aggregation Models loaded', color: "#982315", error: false
            });
        }, error => console.error(error));
    }
    public refresh(): void {

        this.httpHandler.get(this.dssFetchUrl).subscribe(result => {
            this.aggrModels = result.json() as AggrModel[];
            this.logOutput.push({
                message: 'Aggregation Models loaded', color: "#982315", error: false});
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


interface AggrModel {
    id: number;
    title: string;
    description: string;
    createdBy: string;
    execute(): void;
    
}
interface LogOutput {

    message: string;
    error: boolean;
    color: string;


}