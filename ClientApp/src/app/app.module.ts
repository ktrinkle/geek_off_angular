import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

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
import { MsalComponent } from './msal-component/msal.component'

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { DataService } from './data.service';
import { StoreModule } from '@ngrx/store';
import { reducers, metaReducers } from './store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { EffectsModule } from '@ngrx/effects';
import { Round2Effects } from './store/round2/round2.effects';
import { Round1Effects } from './store/round1/round1.effects';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { IPublicClientApplication, PublicClientApplication, InteractionType } from '@azure/msal-browser';


// MSAL config
import { MsalGuard, MsalBroadcastService, MsalModule, MsalService, MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG, MsalGuardConfiguration, MsalRedirectComponent, MsalInterceptor, MsalInterceptorConfiguration } from '@azure/msal-angular';
import { msalConfig, protectedResources } from '../auth/auth-config';

// Material
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTableModule } from '@angular/material/table';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { PregameComponent } from './control/pregame/pregame.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonToggle, MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDialog, MatDialogContent, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { CountdownModule } from 'ngx-countdown';
import { Round2countdowndialogComponent } from './round2/round2countdowndialog/round2countdowndialog.component';
import { Round3scoreboardComponent } from './round3/scoreboard/round3scoreboard.component';
import { DisplayTeamInfoPipe, Round3controlComponent } from './control/round3control/round3control.component';

/**
 * Here we pass the configuration parameters to create an MSAL instance.
 * For more info, visit: https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-angular/docs/v2-docs/configuration.md
 */
export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication(msalConfig);
}

/**
 * MSAL Angular will automatically retrieve tokens for resources
 * added to protectedResourceMap. For more info, visit:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-angular/docs/v2-docs/initialization.md#get-tokens-for-web-api-calls
 */
export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();

  protectedResourceMap.set(protectedResources.geekOffApi.endpoint, protectedResources.geekOffApi.scopes);

  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap
  };
}

/**
 * Set your default interaction type for MSALGuard here. If you have any
 * additional scopes you want the user to consent upon login, add them here as well.
 */
export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: ['user.read']
    },
  };
}

export function MSALInterceptorFactory(): MsalInterceptorConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap: new Map([
      ['https://graph.microsoft.com/v1.0/me', ['user.read']]
    ])
  }
}

@NgModule({
  declarations: [
    AppComponent,
    MsalComponent,
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
    DisplayTeamInfoPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    StoreModule.forRoot(reducers, { metaReducers }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    EffectsModule.forRoot([Round2Effects, Round1Effects]),
    ReactiveFormsModule,
    MsalModule,
    MatToolbarModule,
    MatTableModule,
    MatListModule,
    MatIconModule,
    BrowserAnimationsModule,
    CountdownModule,
    MatGridListModule,
    MatButtonModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    MatSlideToggleModule,
    MatButtonToggleModule,
    MatButtonToggleModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule
  ],
  exports: [
    FormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MSALGuardConfigFactory
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory
    },
    MsalService,
    MsalGuard,
    MsalBroadcastService,
    DataService,
    Round2controlComponent
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule {

  ngDoBootstrap(ref: any) {
    if (window !== window.parent && !window.opener) {
      console.log("Bootstrap: MSAL");
      ref.bootstrap(MsalComponent);
    }
    else {
      //this.router.resetConfig(RouterModule);
      console.log("Bootstrap: App");
      ref.bootstrap(AppComponent);
    }
  }
}
