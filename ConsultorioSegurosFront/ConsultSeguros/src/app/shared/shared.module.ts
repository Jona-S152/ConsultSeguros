import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { SearchBoxComponent } from './components/search-box/search-box.component';
import { MaterialModule } from '../material/material.module';


@NgModule({
  declarations: [
    SearchBoxComponent
  ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    MaterialModule
  ],
  exports: [
    SearchBoxComponent
  ]
})
export class SharedModule { }
