import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

import { Round2controlComponent } from './control/round2control/round2control.component';
import { Round2hostComponent } from './host/round2host/round2host.component';
import { Round2countdownComponent } from './round2/round2countdown/round2countdown.component';
import { Round2displayComponent } from './round2/round2display/round2display.component';

import { Round1ContestantComponent } from './round1/contestant/contestant.component';
import { Round1IntroComponent } from './round1/intro/intro.component';
import { Round1DisplayQuestionComponent } from './round1/display-question/display-question.component';
import { Round1ScoreboardComponent } from './round1/scoreboard/scoreboard.component';

import { MsalGuard, MsalRedirectComponent } from '@azure/msal-angular';
import { BrowserUtils } from '@azure/msal-browser';

const routes: Routes = [
  {
    // Dedicated route for redirects
    path: 'code',
    component: MsalRedirectComponent
  },
  // guards for what we see below need to be added
  {
    path: 'round1/contestant',
    component: Round1ContestantComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  {
    path: 'round1/intro/:page',
    component: Round1IntroComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  {
    path: 'round1/question/:question',
    component: Round1DisplayQuestionComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  {
    path: 'round1/scoreboard',
    component: Round1ScoreboardComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  {
    path: 'round2/countdown',
    component: Round2countdownComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  {
    path: 'round2/display',
    component: Round2displayComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  {
    path: 'control/round1',
    component: Round2controlComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  {
    path: 'control/round2',
    component: Round2controlComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  {
    path: 'host/round2',
    component: Round2hostComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  // these do not need MSAL guards and should be open to all
  {
    path: 'home',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: true,
    // Don't perform initial navigation in iframes or popups
    initialNavigation: !BrowserUtils.isInIframe() && !BrowserUtils.isInPopup() ? 'enabled' : 'disabled'
  })],
  exports: [RouterModule],

})
export class AppRoutingModule { }
