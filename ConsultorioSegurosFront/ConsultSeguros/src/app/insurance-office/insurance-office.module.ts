import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InsuranceOfficeRoutingModule } from './insurance-office-routing.module';
import { InsurancePageComponent } from './insurance-page/insurance-page.component';
import { InsuredPageComponent } from './insured-page/insured-page.component';
import { ListInsurancePageComponent } from './list-insurance-page/list-insurance-page.component';
import { ListInsuredPageComponent } from './list-insured-page/list-insured-page.component';


@NgModule({
  declarations: [
    InsurancePageComponent,
    InsuredPageComponent,
    ListInsurancePageComponent,
    ListInsuredPageComponent
  ],
  imports: [
    CommonModule,
    InsuranceOfficeRoutingModule
  ]
})
export class InsuranceOfficeModule { }
