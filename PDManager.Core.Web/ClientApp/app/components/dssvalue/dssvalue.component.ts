import { Component, OnInit, Inject  } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
@Component({
    selector: 'dssvalue',
    templateUrl: './dssvalue.component.html'
})

export class DSSValueComponent implements OnInit {
    public dssConfig: DSSConfig;
    public logOutput: string;
    public patientId: string;
    public dssOutput: DSSOutputValue[];
    public dssInput: DSSValue[];
    private httpClient: Http;
    private baseUrl: string;
    private modelId: string;
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
    getModel():void {


        var url = this.baseUrl + 'api/v1/dss/config/' + this.modelId;

        console.log(url);

        this.httpClient.get(url).subscribe(result => {
            console.log(result.json());
            this.dssConfig = result.json() as DSSConfig;
            this.dssInput = this.dssConfig.input;
        }, error => console.error(error));


    }

    onSubmit(form: any): void {


        var url = this.baseUrl + 'api/v1/dsseval';

        var model = { 'ModelId': this.modelId, 'Input': JSON.stringify(form) };
      
        var request = this.httpClient.post(url, model).subscribe(
            res => {
                console.log(res);
                this.dssOutput = res.json() as DSSOutputValue[];
            },
            err => {
                console.log("Error occured");
            }
        );
        
        
    }


    getData(): void {
        var url = this.baseUrl + 'api/v1/dsseval/input/' + this.modelId+'?patientId='+this.patientId;
        var request = this.httpClient.get(url).subscribe(res => {
            console.log(res);
            var self = this;
            var sourceInput = res.json() as DSSValue[];

            var newInput = Object.assign(this.dssInput) as DSSValue[];

            sourceInput.forEach(function (newi) {

                var dssI = newInput.filter(function (item) {
                    return item.name == newi.name;
                }).forEach(function (newitem) {

                    newitem.value = newi.value;                 
                })
                
                
            });
            this.dssInput = newInput;

        },
            err => {
                console.log("Error occured");
            }
        );


    }

}

interface DSSConfig {
    version: number;
    name: string;    
    input: DSSValue[];
}


interface DSSValue {
    value: number;
    name: string;
    numeric: boolean;
    code: string;
    categoryMapping: DSSValueCategory[];
}


interface DSSValueCategory {
    value: number;
    name: string;

}

interface DSSOutputValue {
    value: string;
    name: string;
    
}
