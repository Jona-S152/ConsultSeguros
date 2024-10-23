import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InsuranceOfficeRoutingModule } from './insurance-office-routing.module';
import { InsurancePageComponent } from './pages/insurance-page/insurance-page.component';
import { InsuredPageComponent } from './pages/insured-page/insured-page.component';
import { ListInsurancePageComponent } from './pages/list-insurance-page/list-insurance-page.component';
import { ListInsuredPageComponent } from './pages/list-insured-page/list-insured-page.component';
import { LayoutPageComponent } from './pages/layout-page/layout-page.component';
import { MaterialModule } from '../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NewInsuranceComponent } from './components/new-insurance/new-insurance.component';
import { SharedModule } from '../shared/shared.module';
import { NewInsuredComponent } from './components/new-insured/new-insured.component';


@NgModule({
  declarations: [
    InsurancePageComponent,
    InsuredPageComponent,
    ListInsurancePageComponent,
    ListInsuredPageComponent,
    LayoutPageComponent,
    NewInsuranceComponent,
    NewInsuredComponent
  ],
  imports: [
    CommonModule,
    InsuranceOfficeRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    SharedModule
  ]
})
export class InsuranceOfficeModule { }
