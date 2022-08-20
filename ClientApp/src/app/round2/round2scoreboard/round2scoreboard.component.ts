import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Store } from '@ngrx/store';
import * as signalR from '@microsoft/signalr';
import { round23Scores } from 'src/app/data/data';
import { environment } from 'src/environments/environment';
import { DataService } from '../../data.service';
import { selectCurrentEvent } from 'src/app/store';
import { takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-round2-scoreboard',
  templateUrl: './round2scoreboard.component.html',
  styleUrls: ['./round2scoreboard.component.scss']
})
export class Round2scoreboardComponent implements OnInit, OnDestroy {
  yEvent = '';
  public roundNum: number = 2;
  public scores: round23Scores[] = [];
  public colors: string[] = [
    'coral',
    'steelblue',
    'gold',
    'lightgreen',
    'sandybrown',
    'plum',
  ]
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private _dataService: DataService, private store: Store, private _router: Router) { }

  ngOnInit(): void {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.getScoreboardInfo(this.yEvent);
      }
    });

    console.log("End of Init");

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(`${environment.api_url}/events`)
      .withAutomaticReconnect()
      .build();

    connection.start().then(() => {
      console.log('SignalR Connected!');
    }).catch(err => {
      return console.error(err.toString());
    });

    connection.on("round2ScoreUpdate", (_: any) => {
      this.getScoreboardInfo(this.yEvent);
    })

    connection.on("round3ScoreUpdate", (_: any) => {
      this._router.navigate(['/round3/scoreboard']);
    })
  }

  public getScoreboardInfo(yevent: string) {
    this._dataService.getRound2Scores(yevent).subscribe((data: round23Scores[]) => {
      this.scores = data.sort((a, b) => a.teamScore ? - (b.teamScore || 0) : 0);

      this.scores.forEach((score, index) => {
        score.color = this.colors[index];
      });

      console.log(this.scores);
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
