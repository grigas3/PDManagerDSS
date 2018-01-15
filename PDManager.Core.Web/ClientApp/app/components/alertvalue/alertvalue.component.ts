import { Component, OnInit, Inject  } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
@Component({
    selector: 'alertvalue',
    templateUrl: './alertvalue.component.html'
})

export class AlertValueComponent implements OnInit {
    public alertOutput: AlertOutput;
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

            this.modelId = params.id;


        });
    }
      
    execute(): void {
        

        var url = this.baseUrl + 'api/v1/alert/evaluate/' + this.modelId + '?patientId=' + this.patientId;
        this.httpClient.get(url).subscribe(result => {
            var self = this;
            this.alertOutput = result.json() as AlertOutput;
            
        }, error => console.error(error));

        
        
    }

   
}

//Aggregation Confi Model
interface AlertOutput {
    message: string;
    level: number;
    color: string;
    
}
