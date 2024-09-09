import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Store } from '@ngrx/store';
import * as signalR from '@microsoft/signalr';
import { round23Scores } from 'src/app/data/data';
import { environment } from 'src/environments/environment';
import { DataService } from '../../data.service';
import { selectCurrentEvent, selectRound3Scores } from 'src/app/store';
import { takeUntil } from 'rxjs/operators';
import { round3Score } from 'src/app/store/round3/round3.actions';

@Component({
  selector: 'app-round3-scoreboard',
  templateUrl: './round3scoreboard.component.html',
  styleUrls: ['./round3scoreboard.component.scss']
})
export class Round3scoreboardComponent implements OnInit, OnDestroy {
  yEvent = '';
  public RoundNum: number = 2;
  public scores: any[] = [];
  public colors: string[] = [
    'red',
    'green',
    'blue',
  ]
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private _dataService: DataService, private store: Store) { }

  ngOnInit(): void {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.getUpdatedScores();
        this.getScoreboardInfo();
      }
    });

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(`${environment.api_url}/events`, { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    connection.start().then(() => {
      console.log('SignalR Connected!');
    }).catch(err => {
      return console.error(err.toString());
    });

    connection.on("round3ScoreUpdate", (_: any) => {
      this.getUpdatedScores();
      this.getScoreboardInfo();
    })
  }

  public getUpdatedScores() {
    this.store.dispatch(round3Score({ yEvent: this.yEvent }));
  }

  public getScoreboardInfo() {
    this.store.select(selectRound3Scores).pipe(takeUntil(this.destroy$)).subscribe((data: round23Scores[]) => {
      console.log('scoreboard update', data);
      this.scores = [];
      data.forEach((score, index) => {
        this.scores.push({
          teamName: score.teamName,
          teamNum: score.teamNum,
          teamScore: score.teamScore,
          color: this.colors[index]
        });
      });
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
