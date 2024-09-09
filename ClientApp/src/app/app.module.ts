import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { JwtModule } from "@auth0/angular-jwt";
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Round2countdownComponent } from './round2/round2countdown/round2countdown.component';
import { Round2displayComponent } from './round2/round2display/round2display.component';
import { Round2hostComponent } from './host/round2host/round2host.component';
import { Round2controlComponent } from './control/round2control/round2control.component';
import { HomeComponent } from './home/home.component';
import { Round1ContestantComponent } from './round1/contestant/contestant.component';
import { Round1ControlComponent } from './control/round1control/round1control.component';
import { Round2scoreboardComponent } from './round2/round2scoreboard/round2scoreboard.component';
import { Round1hostComponent } from './host/round1host/round1host.component';
import { Round1IntroComponent } from './round1/intro/intro.component';
import { Round1DisplayQuestionComponent } from './round1/display-question/display-question.component';
import { Round1ScoreboardComponent } from './round1/scoreboard/scoreboard.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { DataService } from './data.service';
import { StoreModule } from '@ngrx/store';
import { reducers, metaReducers } from './store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { EffectsModule } from '@ngrx/effects';
import { EventManageEffects } from './store/eventManage/eventManage.effects';
import { Round3Effects } from './store/round3/round3.effects';
import { Round2Effects } from './store/round2/round2.effects';
import { Round1Effects } from './store/round1/round1.effects';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Material
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTableModule } from '@angular/material/table';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { PregameComponent } from './control/pregame/pregame.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { CountdownModule } from 'ngx-countdown';
import { Round2countdowndialogComponent } from './round2/round2countdowndialog/round2countdowndialog.component';
import { Round3scoreboardComponent } from './round3/scoreboard/round3scoreboard.component';
import { DisplayTeamInfoPipe, Round3controlComponent } from './control/round3control/round3control.component';
import { TeamsetupComponent } from './eventsetup/teamsetup/teamsetup.component';
import { TeamstatsComponent } from './eventsetup/teamstats/teamstats.component';
import { TeamlinkComponent } from './eventsetup/teamlink/teamlink.component';
import { EventchooserComponent } from './eventsetup/eventchooser/eventchooser.component';
import { AdminComponent } from './login/admin/admin.component';
import { PlayerComponent } from './login/player/player.component';

import { QRCodeModule } from 'angularx-qrcode';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    Round2countdownComponent,
    Round2displayComponent,
    Round2hostComponent,
    Round2controlComponent,
    Round2scoreboardComponent,
    HomeComponent,
    Round1ContestantComponent,
    Round1IntroComponent,
    Round1DisplayQuestionComponent,
    Round1ScoreboardComponent,
    Round1ControlComponent,
    PregameComponent,
    Round1hostComponent,
    Round2countdowndialogComponent,
    Round3scoreboardComponent,
    Round3controlComponent,
    DisplayTeamInfoPipe,
    TeamsetupComponent,
    TeamstatsComponent,
    TeamlinkComponent,
    EventchooserComponent,
    AdminComponent,
    PlayerComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5000", "localhost:4200", "geekoff.azurewebsites.net"]
      },
    }),
    HttpClientModule,
    StoreModule.forRoot(reducers, { metaReducers }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    EffectsModule.forRoot([EventManageEffects, Round3Effects, Round2Effects, Round1Effects]),
    ReactiveFormsModule,
    MatToolbarModule,
    MatTableModule,
    MatListModule,
    MatIconModule,
    BrowserAnimationsModule,
    CountdownModule,
    MatGridListModule,
    MatButtonModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    MatButtonToggleModule,
    MatButtonToggleModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    QRCodeModule,
  ],
  exports: [
    FormsModule
  ],
  providers: [
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: MsalInterceptor,
    //   multi: true
    // },
    DataService,
    Round2controlComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  ngDoBootstrap(ref: any) {

      console.log("Bootstrap: App");
      ref.bootstrap(AppComponent);

  }
}
