import { Component, OnInit, OnDestroy } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { Subject } from 'rxjs';
import { takeUntil, map } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { round1Scores, round1ScoreMap } from 'src/app/data/data';
import { Router } from '@angular/router';

@Component({
  selector: 'app-scoreboard',
  templateUrl: './scoreboard.component.html',
  styleUrls: ['./scoreboard.component.scss']
})
export class Round1ScoreboardComponent implements OnInit {

  yevent: string = sessionStorage.getItem('event') ?? '';
  scoreObject: round1Scores[] = [];
  displayObject: round1ScoreMap[] = [];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private dataService: DataService, private router:Router) { }

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
    console.log(this.yevent);
    this.dataService.getRound1Scores(this.yevent).pipe(takeUntil(this.destroy$)).subscribe(s => {
      this.displayObject.push(
        {
          teamNum: s.teamNum,
          teamName: s.teamName,
          q: s.q.map((q: { questionId: number; questionScore: string; }) => [q.questionId, q.questionScore]),
          bonus: s.bonus,
          teamScore: s.teamScore
        }
      );
    });

    console.log(this.displayObject);
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
