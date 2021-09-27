import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Round2controlComponent } from './control/round2control/round2control.component';
import { HomeComponent } from './home/home.component';
import { Round2hostComponent } from './host/round2host/round2host.component';
import { Round2countdownComponent } from './round2/round2countdown/round2countdown.component';
import { Round2displayComponent } from './round2/round2display/round2display.component';

import { Round1ContestantComponent } from './round1/contestant/contestant.component';

import { MsalGuard } from '@azure/msal-angular';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, pathMatch: 'full' },
  // guards for what we see below need to be added
  { path: 'round1/contestant',
    component: Round1ContestantComponent,
    pathMatch: 'full',
    canActivate: [MsalGuard]
  },
  { path: 'round2/countdown', component: Round2countdownComponent, pathMatch: 'full' },
  { path: 'round2/display', component: Round2displayComponent, pathMatch: 'full' },
  { path: 'control/round2', component: Round2controlComponent, pathMatch: 'full' },
  { path: 'host/round2', component: Round2hostComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
