import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DataService } from 'src/app/data.service';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { currentQuestionDto, round1QDisplay } from 'src/app/data/data';
import { Store } from '@ngrx/store';
import { selectCurrentEvent, selectRound1BigDisplay } from 'src/app/store';
import { round1BigDisplay } from 'src/app/store/round1/round1.actions';

@Component({
  selector: 'app-display-question',
  templateUrl: './display-question.component.html',
  styleUrls: ['./display-question.component.scss'],
  animations: [
    trigger('fadeIn', [
      state('void', style({
        opacity: 0
      })),
      transition('void => *', animate(1000)),
    ]),
    trigger('slideLeft', [
      transition(':enter', [style({ width: 0 }), animate(300)]),
      transition(':leave', [animate(300, style({ width: 0 }))]),
    ]),
  ]
})

export class Round1DisplayQuestionComponent implements OnInit, OnDestroy {
  // internal management since users won't leave this page
  bigDisplay: round1QDisplay[] = [];
  questionVisible: boolean = false;
  mediaVisible: boolean = false;
  answerVisible: boolean = false;
  answerShown: boolean = false;
  currentScreen: string = "question";
  matchString = 'xABCD'; // 1 based to make life easier ahead
  currentQuestion: currentQuestionDto = {
    questionNum: 0,
    status: 0
  };
  yEvent: string = '';
  public currentQuestionDto: round1QDisplay = {
    questionNum: 0,
    questionText: '',
    answers: [],
    correctAnswer: '',
    answerType: -1,
    mediaFile: '',
    mediaType: ''
  };
  questionMatch: number = 1;
  questionMulti: number = 0;
  questionText: number = 2;
  destroy$: Subject<boolean> = new Subject<boolean>();

  // hack to force the text size smaller if the question is >110 chars
  longText: string = '';

  constructor(private route: ActivatedRoute, private router: Router, private dataService: DataService, private store: Store) { }

  ngOnInit(): void {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.store.dispatch(round1BigDisplay({ yEvent: this.yEvent }));
      }
    });

    this.store.select(selectRound1BigDisplay).pipe(takeUntil(this.destroy$)).subscribe(bigDisplay =>
      this.bigDisplay = bigDisplay as round1QDisplay[]
    );

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

    connection.on("round1UpdateContestant", (data: currentQuestionDto) => {
      this.currentQuestion = data;
      this.loadCurrentQuestion(data.questionNum);

      if (data.questionNum > 0 && data.questionNum < 120) {
        // we have already loaded in the init so we have our state here. In theory.
        if (data.status == 3) {
          this.showAnswer();
        }

        if (data.status == 1 || data.status == 2) {
          this.showChoices();
        }
      };
    });

    connection.on("round1intro", (data: any) => {
      this.changePage(data);
    });

    connection.on("round2ChangeTeam", (data: any) => {
      this.router.navigate(['/round2feud/display']);
    });
  }

  loadCurrentQuestion(questionNum: number) {
    this.currentQuestion = {
      questionNum: questionNum,
      status: 0
    };

    this.questionVisible = true;
    this.answerVisible = false;
    this.answerShown = false;
    this.longText = '';
    const currentQuestion = this.bigDisplay.filter(x => x.questionNum == questionNum);
    if (currentQuestion.length > 0) {
      this.currentQuestionDto = currentQuestion[0];
      if (currentQuestion[0].questionText.length > 100) {
        this.longText = 'longQuestion';
      }
      if (this.currentQuestionDto.answerType == 1) {
        this.matchString = 'x' + this.convertStringToAnswer(this.currentQuestionDto.correctAnswer);
      }
    }
    this.debugVals();
  }

  showChoices() {
    this.questionVisible = true;
    this.answerVisible = true;
    this.answerShown = false;
    this.currentQuestion.status = 1;
    this.debugVals();
  }

  showAnswer() {
    this.questionVisible = true;
    this.answerVisible = true;
    this.answerShown = true;
    this.currentQuestion.status = 3;
    this.debugVals();
  }

  showMedia() {
    this.mediaVisible = true;
  }

  changePage(page: any): void {
    console.log('ChangePage: ' + page);
    this.currentScreen = page;
  }

  debugVals() {
    console.log("questionVisible " + this.questionVisible);
    console.log("mediavisible" + this.mediaVisible);
    console.log("answerVisible " + this.answerVisible);
    console.log("answerShown" + this.answerShown);
    console.log(this.currentQuestionDto);
  }

  convertStringToAnswer(input: string): string {
    // shift all ascii bytes up by 16 to convert numbers to letters
    let newS = '';
    for (let i = 0; i < input.length; ++i) {
      // ASCII value
      let val = input[i].charCodeAt(0);
      newS += String.fromCharCode(val + 16);
    }
    return newS;
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
