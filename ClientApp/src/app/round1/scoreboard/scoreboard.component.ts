/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit, OnDestroy } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectCurrentEvent } from 'src/app/store';


@Component({
  selector: 'app-scoreboard',
  templateUrl: './scoreboard.component.html',
  styleUrls: ['./scoreboard.component.scss']
})
export class Round1ScoreboardComponent implements OnInit, OnDestroy {

  yEvent = '';
  headers: string[] = [];
  teamData: any[] = [];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private dataService: DataService, private router: Router, private store: Store) { }

  ngOnInit(): void {
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      this.getScoreboard();
    });

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events', { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round1ScoreUpdate", () => {
      console.log('Updating scoreboard');
      this.getScoreboard();
    });

    connection.on("round2ScoreUpdate", () => {
      this.router.navigate(['/round2feud/scoreboard']);
    });

  }

  getScoreboard(): void {
    if (this.yEvent && this.yEvent.length > 0) {
      this.dataService.getRound1Scores(this.yEvent).pipe(takeUntil(this.destroy$)).subscribe(s => {
        let questionNumbers: any[] = [];
        if (s.length > 0) {
          for (const team of s) {
            const tempNumbers = [...new Set(team.q.map((q: any) => q.questionNum))];
            questionNumbers = [...new Set([...questionNumbers, ...tempNumbers])]
          }
        }

        // this creates the headers to display
        this.headers = ['#']
        this.headers.push('NAME');
        if (questionNumbers.length > 0) {
          for (const number of questionNumbers) {
            if (number > 15) {
              this.headers.push('T' + (number - 15));
            }
            else {
              this.headers.push(number);
            }
          }
        }
        this.headers.push('BNS');
        this.headers.push('TTL');

        // reset teamData
        this.teamData = [];

        for (const team of s) {
          const temp = [team.teamNum, team.teamName];
          if (questionNumbers) {
            for (const number of questionNumbers) {
              const question = team.q.filter((a: any) => a.questionNum === number);
              if (question.length > 0) {
                temp.push(question[0].questionScore);
              } else {
                temp.push(0);
              }
            }
            temp.push(team.bonus);
            temp.push(team.teamScore);
          }

          this.teamData.push(temp);
        }
      });
    }
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
