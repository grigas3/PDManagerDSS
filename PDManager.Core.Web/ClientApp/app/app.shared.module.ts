import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ChartsModule } from 'ng2-charts';
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { DSSValueComponent } from './components/dssvalue/dssvalue.component';
import { AggrValueComponent } from './components/aggrvalue/aggrvalue.component';
import { DSSComponent } from './components/dss/dss.component';
import { AggregationComponent } from './components/aggregation/aggr.component';
@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        DSSValueComponent,        
        DSSComponent,
        AggregationComponent,
        AggrValueComponent,        
        HomeComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ChartsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'dssvalue/:id', component: DSSValueComponent },
            { path: 'aggrvalue/:id', component: AggrValueComponent },
            { path: 'dss', component: DSSComponent },     
            { path: 'aggregation', component: AggregationComponent },     
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
