import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/service/auth.service';
import { Router, ActivatedRoute} from '@angular/router';
import { Guid } from 'typescript-guid';
import { DataService } from 'src/app/data.service';
import { teamLogin } from 'src/app/data/data';

@Component({
  selector: 'app-player-login',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit {

  private yEvent: string | undefined;
  private teamGuid: Guid | undefined;

  constructor(private authService: AuthService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {

    // get router params
    this.route.queryParams.subscribe(params => {
      this.yEvent = params['yEvent'];
      this.teamGuid = params['teamGuid'];
    });

    if (this.yEvent && this.teamGuid)
    {
      const teamLogin: teamLogin = {
        yEvent: this.yEvent,
        teamGuid: this.teamGuid
      }

      // start the signin process
      var success = this.authService.processLoginTeam(teamLogin);
      if (success)
      {
        this.router.navigate(['/round1/contestant']);
      }
      else
      {
        this.router.navigate(['home']);
      }

    }


  }

}
