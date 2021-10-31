import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Store } from '@ngrx/store';
import * as signalR from '@microsoft/signalr';
import { round23Scores } from 'src/app/data/data';
import { environment } from 'src/environments/environment';
import { DataService } from '../../data.service';
import { selectCurrentEvent } from 'src/app/store';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-round3-scoreboard',
  templateUrl: './round3scoreboard.component.html',
  styleUrls: ['./round3scoreboard.component.scss']
})
export class Round3scoreboardComponent implements OnInit, OnDestroy {
  yEvent = '';
  public roundNo: number = 2;
  public scores: round23Scores[] = [];
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

    connection.on("round3ScoreUpdate", (_: any) => {
      this.getScoreboardInfo(this.yEvent);
    })
  }

  public getScoreboardInfo(yevent: string) {
    this._dataService.getRound2Scores(yevent).subscribe((data: round23Scores[]) => {

      this.scores = data;
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
