import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTable } from '@angular/material/table';
import { Store } from '@ngrx/store';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DataService } from 'src/app/data.service';
import { round2Answers, round2Display, round2SurveyList } from 'src/app/data/data';
import { selectRound2AllSurvey } from 'src/app/store';
import { round2AllSurvey } from 'src/app/store/round2/round2.actions';

export interface displayRow {
  player1: round2Answers;
  player2: round2Answers;
}
@Component({
  selector: 'app-round2display',
  templateUrl: './round2display.component.html',
  styleUrls: ['./round2display.component.scss']
})
export class Round2displayComponent implements OnInit, OnDestroy {
  @ViewChild(MatTable, { static: true }) table!: MatTable<any>;
  destroy$: Subject<boolean> = new Subject<boolean>();
  yEvent = sessionStorage.getItem('event') ?? '';
  teamNumber = 1;
  displayObject: round2Display = {
    teamNo: 0,
    player1Answers: [],
    player2Answers: [],
    finalScore: 0
  };

  displayRows: displayRow[] = [];


  constructor(private dataService: DataService) {
  }

  ngOnInit(): void {
    this.dataService.getRound2DisplayBoard(this.yEvent, this.teamNumber).pipe(takeUntil(this.destroy$)).subscribe(x => {
      this.teamNumber = x.teamNo;
      let questionNumbers = [...new Set([...x.player1Answers.map((q: round2Answers) => q.questionNum), ...x.player2Answers.map((q: round2Answers) => q.questionNum)])];
      questionNumbers = questionNumbers.sort((a, b) => (a > b) ? 1 : -1);
      for (let questionNumber of questionNumbers) {
        const player1Answer = x.player1Answers.filter((x: { questionNum: string; }) => x.questionNum === questionNumber);
        const player2Answer = x.player2Answers.filter((x: { questionNum: string; }) => x.questionNum === questionNumber);
        this.displayRows.push(
          {
            player1: player1Answer.length > 0 ? player1Answer[0] : { questionNum: questionNumber, answer: '', score: null },
            player2: player2Answer.length > 0 ? player2Answer[0] : { questionNum: questionNumber, answer: '', score: null }
          });
      }
      this.table.renderRows();
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

}
