import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

import { Round2controlComponent } from './control/round2control/round2control.component';
import { Round2hostComponent } from './host/round2host/round2host.component';
import { Round2countdownComponent } from './round2/round2countdown/round2countdown.component';
import { Round2displayComponent } from './round2/round2display/round2display.component';
import { Round2scoreboardComponent } from './round2/round2scoreboard/round2scoreboard.component';

import { PregameComponent } from './control/pregame/pregame.component';
import { Round1ContestantComponent } from './round1/contestant/contestant.component';
import { Round1IntroComponent } from './round1/intro/intro.component';
import { Round1DisplayQuestionComponent } from './round1/display-question/display-question.component';
import { Round1ScoreboardComponent } from './round1/scoreboard/scoreboard.component';
import { Round1ControlComponent } from './control/round1control/round1control.component';
import { Round1hostComponent } from './host/round1host/round1host.component';
import { Round3scoreboardComponent } from './round3/scoreboard/round3scoreboard.component';
import { Round3controlComponent } from './control/round3control/round3control.component';
import { TeamstatsComponent } from './eventsetup/teamstats/teamstats.component';
import { EventchooserComponent } from './eventsetup/eventchooser/eventchooser.component';

import { PlayerGuard } from './player.guard';
import { PlayerComponent } from './login/player/player.component';

const roles = {
  "Player": "player",
  "Admin": "admin",
  "Host": "host"
}

const routes: Routes = [
  // guards for what we see below need to be added
  {
    path: 'round1/contestant',
    component: Round1ContestantComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Player
    }
  },
  {
    path: 'round1/intro/:page',
    component: Round1IntroComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'round1/question/:question',
    component: Round1DisplayQuestionComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'round1/scoreboard',
    component: Round1ScoreboardComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'round2feud/countdown',
    component: Round2countdownComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'round2feud/display',
    component: Round2displayComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'control/pregame',
    component: PregameComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'round3/scoreboard',
    component: Round3scoreboardComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'round2feud/scoreboard',
    component: Round2scoreboardComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'round2feud/scoreboard',
    component: Round2scoreboardComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard]
  },
  {
    path: 'round2feud/scoreboard',
    component: Round2scoreboardComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard]
  },
  {
    path: 'control/round1',
    component: Round1ControlComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'control/round2feud',
    component: Round2controlComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'control/round3',
    component: Round3controlComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'host/round2feud',
    component: Round2hostComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'host/round1',
    component: Round1hostComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'manageevent/stats',
    component: TeamstatsComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  {
    path: 'manageevent/setevent',
    component: EventchooserComponent,
    pathMatch: 'full',
    canActivate: [PlayerGuard],
    data: {
      expectedRole: roles.Admin
    }
  },
  // these do not need guards and should be open to all
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: 'player/:yevent/:teamGuid',
    component: PlayerComponent,
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent,
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: false,
    // Don't perform initial navigation in iframes or popups
  })],
  exports: [RouterModule],

})
export class AppRoutingModule { }
