import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { round23Scores } from 'src/app/data/data';
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
  }

  public async getScoreboardInfo(yevent: string) {
    await this._dataService.getRound2Scores(yevent).subscribe((data: round23Scores[]) => {
      this.scores = data.sort((a, b) => a.teamScore? - (b.teamScore || 0) : 0);

      this.scores.forEach((score, index) => {
        score.color = this.colors[index];
      });

      console.log(this.scores);
    });
  }
}
