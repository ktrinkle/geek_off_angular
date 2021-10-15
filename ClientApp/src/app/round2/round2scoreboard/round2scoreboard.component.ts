import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { round23Scores } from 'src/app/data/data';
import { environment } from 'src/environments/environment';
import { DataService } from '../../data.service';

@Component({
  selector: 'app-round2-scoreboard',
  templateUrl: './round2scoreboard.component.html',
  styleUrls: ['./round2scoreboard.component.scss']
})
export class Round2scoreboardComponent implements OnInit {

  constructor(private _dataService: DataService) { }

  yEvent = sessionStorage.getItem('event') ?? '';
  public roundNo: number = 2;
  public scores: round23Scores[] = [];
  public colors: string[] = [
    'coral',
    'steelblue',
    'gold',
    'lightgreen',
    'sandybrown',
    'plum',
  ]

  ngOnInit(): void {
    this.getScoreboardInfo(this.yEvent);
    console.log("End of Init");

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(`${environment.api_url}/events`)
      .build();

    connection.start().then(() => {
      console.log('SignalR Connected!');
    }).catch(err => {
      return console.error(err.toString());
    });

    connection.on("round2ScoreUpdate", (_: any) => {
      this.getScoreboardInfo(this.yEvent);
    })
  }

  public getScoreboardInfo(yevent: string) {
    this._dataService.getRound2Scores(yevent).subscribe((data: round23Scores[]) => {
      this.scores = data.sort((a, b) => a.teamScore? - (b.teamScore || 0) : 0);

      this.scores.forEach((score, index) => {
        score.color = this.colors[index];
      });

      console.log(this.scores);
    });
  }
}
