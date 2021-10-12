import { Component, OnInit, OnDestroy } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-scoreboard',
  templateUrl: './scoreboard.component.html',
  styleUrls: ['./scoreboard.component.scss']
})
export class Round1ScoreboardComponent implements OnInit {

  yevent: string = sessionStorage.getItem('event') ?? '';
  headers: { property: string, display: string }[] = [];
  displayHeaders: string[] = [];
  teamData: any[] = [];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private dataService: DataService, private router: Router) { }

  ngOnInit(): void {
    this.getScoreboard();

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events')
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round1ScoreUpdate", (data: any) => {
      this.getScoreboard();
    });

    connection.on("round2ScoreUpdate", (data: any) => {
      this.router.navigate(['/round2/scoreboard']);
    });

  }

  getScoreboard(): void {
    this.dataService.getRound1Scores(this.yevent).pipe(takeUntil(this.destroy$)).subscribe(s => {
      let questionNumbers = null;
      if (s.length > 0) {
        questionNumbers = [...new Set(s[0].q.map((q: any) => q.questionId))];
      }

      // this creates the headers to display
      this.headers = [
        {
          property: 'teamNumber',
          display: '#'
        },
        {
          property: 'teamName',
          display: 'NAME'
        }
      ];
      if (questionNumbers) {
        for (const number of questionNumbers) {
          this.headers.push({ property: `${number}`, display: `${number}` });
        }
      }
      this.headers.push({
        property: 'bonus',
        display: 'BONUS'
      });
      this.headers.push({
        property: 'total',
        display: 'TTL'
      });
      this.displayHeaders = this.headers.map(h => h.display);

      for (let team of s) {
        let temp = {
          teamNumber: team.teamNum,
          teamName: team.teamName,
          bonus: team.bonus,
          total: team.teamScore
        }
        if (questionNumbers) {
          for (const number of questionNumbers) {
            const question = team.q.filter((a: any) => a.questionId === number);
            if (question.length > 0) {
              Object.assign(temp, { [`${number}`]: question[0].questionScore });
            } else {
              Object.assign(temp, { [`${number}`]: 0 });
            }
          }
        }

        this.teamData.push(temp);
      }

    });
  };

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
