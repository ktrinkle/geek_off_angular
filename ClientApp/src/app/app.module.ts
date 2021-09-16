import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Round2countdownComponent } from './round2/round2countdown/round2countdown.component';
import { Round2displayComponent } from './round2/round2display/round2display.component';
import { Round2hostComponent } from './host/round2host/round2host.component';
import { Round2controlComponent } from './control/round2control/round2control.component';
import { HomeComponent } from './home/home.component';
import { DataService } from './data.service';

@NgModule({
  declarations: [
    AppComponent,
    Round2countdownComponent,
    Round2displayComponent,
    Round2hostComponent,
    Round2controlComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
