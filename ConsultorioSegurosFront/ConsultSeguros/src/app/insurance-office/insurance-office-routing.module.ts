import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListInsurancePageComponent } from './pages/list-insurance-page/list-insurance-page.component';
import { ListInsuredPageComponent } from './pages/list-insured-page/list-insured-page.component';
import { InsurancePageComponent } from './pages/insurance-page/insurance-page.component';
import { InsuredPageComponent } from './pages/insured-page/insured-page.component';
import { LayoutPageComponent } from './pages/layout-page/layout-page.component';
const routes: Routes = [
  {
    path: '',
    component : LayoutPageComponent,
    children: [
      { path: 'seguros', component: ListInsurancePageComponent },
      { path: 'asegurados', component: ListInsuredPageComponent },
      { path: 'seguro/:id', component: InsurancePageComponent },
      { path: 'asegurado/:id', component: InsuredPageComponent },
      { path: '**', redirectTo: 'list-products'}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InsuranceOfficeRoutingModule { }
