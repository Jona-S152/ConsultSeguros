import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Error404PageComponent } from './shared/pages/error404-page/error404-page.component';
import { InsurancePageComponent } from './insurance-page/insurance-page.component';

@NgModule({
  declarations: [
    AppComponent,
    Error404PageComponent,
    InsurancePageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
