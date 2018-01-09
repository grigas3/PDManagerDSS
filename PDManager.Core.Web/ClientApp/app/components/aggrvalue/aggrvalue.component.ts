import { Component, OnInit, Inject  } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
@Component({
    selector: 'aggrvalue',
    templateUrl: './aggrvalue.component.html'
})

export class AggrValueComponent implements OnInit {
    public aggrConfig: AggrConfig;
    public aggrOutput: PDObservation;
    public patientId: string;
    private httpClient: Http;
    private baseUrl: string;
    private modelId: string;    
    private code: string;


    constructor(private route: ActivatedRoute,http: Http, @Inject('BASE_URL') baseUrl: string) {


        this.baseUrl = baseUrl;
        this.httpClient = http;
       
       
    }

    ngOnInit() {

        this.route.params.subscribe(params => {
            console.log(params.id);
            this.modelId = params.id; 
            this.getModel();
          
        });


    }
    getModel(): void {


        var url = this.baseUrl + 'api/v1/aggregation/config/' + this.modelId;

        console.log(url);

        this.httpClient.get(url).subscribe(result => {
            console.log(result.json());
            this.aggrConfig = result.json() as AggrConfig;
            this.code = this.aggrConfig.code;
            console.log(this.aggrConfig);
            console.log(this.code);
        }, error => console.error(error));


    }
 
    execute(): void {


        var url = this.baseUrl + 'api/v1/aggregation/evaluate?code=' + this.code + '&patientId=' + this.patientId;        
      
        this.httpClient.get(url).subscribe(result => {
            console.log(result.json());
            this.aggrOutput = result.json();
        }, error => console.error(error));
        
        
    }

}

//Aggregation Confi Model
interface AggrConfig {
    version: number;
    name: string;
    code: string;
}

//Aggregation Confi Model
interface PDObservation {
    patientId: string;
    codeid: string;
    timestamp: string;
    id: string;
    value: string;
}
