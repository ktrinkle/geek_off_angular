import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Store } from '@ngrx/store';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DataService } from 'src/app/data.service';
import { round2Answers, round2Display, round2SurveyList } from 'src/app/data/data';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { selectCurrentEvent } from 'src/app/store';

export interface displayRow {
  player1: round2Answers;
  player2: round2Answers;
}

@Component({
  selector: 'app-round2display',
  templateUrl: './round2display.component.html',
  styleUrls: ['./round2display.component.scss'],
  animations: [
    trigger('fadeInOut', [
      state('void', style({
        opacity: 0
      })),
      transition('void <=> *', animate(500)),
    ]),
    trigger('flipState', [
      state('active', style({
        transform: 'rotateY(179.9deg)'
      })),
      state('inactive', style({
        transform: 'rotateY(0)'
      })),
      transition('active => inactive', animate('500ms ease-out')),
      // transition('inactive => active', animate('500ms ease-in'))
      transition('inactive => active', animate('500ms ease-in'))
    ]),
    trigger('slideLeft', [
      transition(':enter', [style({ width: 0 }), animate(300)]),
      transition(':leave', [animate(300, style({ width: 0 }))]),
    ]),
  ]
})

export class Round2displayComponent implements OnInit, OnDestroy {

  destroy$: Subject<boolean> = new Subject<boolean>();
  yEvent = '';
  displayStatus = 0;
  teamNumber = 2;
  totalScore = 0;
  showTotalPlayer1 = 10;
  showTotalPlayer2 = 20;
  displayObject: round2Display = {
    teamNo: 0,
    player1Answers: [],
    player2Answers: [],
    finalScore: 0
  };
  currentScreen = 'question';
  displayRows: displayRow[] = [];


  constructor(private dataService: DataService, private store: Store) {
  }

  ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events')
      .withAutomaticReconnect()
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round2ChangeTeam", (data: any) => {
      console.log('round2ChangeTeam');
      console.log(data);
      this.changeTeam(data);
    });

    connection.on("round2Answer", (data: any) => {
      this.getDisplayBoard();
    });

    connection.on("round2AnswerShow", (data: any) => {
      this.changeDisplayState(data);
    });

    connection.on("round2ShowPlayer1", (data: any) => {
      this.revealPlayerOne();
    });

    connection.on("round2ChangePage", (data: any) => {
      this.changePage(data);
    });

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.getDisplayBoard();
      }
    });
  }

  getDisplayBoard(): void {
    this.dataService.getRound2DisplayBoard(this.yEvent, this.teamNumber).pipe(takeUntil(this.destroy$)).subscribe(x => {
      this.displayRows = [];
      this.teamNumber = x.teamNo;
      this.totalScore = x.finalScore;
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
      };
      console.log(this.displayRows);
    });
  }

  changeDisplayState(state: number): void {
    this.displayStatus = state;
    console.log(this.displayStatus);
  }

  revealPlayerOne() {
    for (let s = 1; s <= 10; s++) {
      this.delay(500).then(() => this.displayStatus = s);
    }
  }

  changeTeam(teamNum: number) {
    console.log(teamNum);
    this.teamNumber = teamNum;
    this.getDisplayBoard();
  }

  changePage(page: any): void {
    this.currentScreen = page;
  }

  async delay(ms: number) {
    await new Promise<void>(resolve => setTimeout(() => resolve(), ms)).then(() => console.log("fired"));
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
