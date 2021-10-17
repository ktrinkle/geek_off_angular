import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DataService } from 'src/app/data.service';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { currentQuestionDto, round1QDisplay } from 'src/app/data/data';

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
  ]
})
export class Round1DisplayQuestionComponent implements OnInit, OnDestroy {
  // internal management since users won't leave this page
  questionVisible:boolean = false;
  mediaVisible: boolean = false;
  answerVisible: boolean = false;
  answerShown: boolean = false;
  currentScreen: string = "question";
  matchString = 'xABCD'; // 1 based to make life easier ahead
  currentQuestion: currentQuestionDto = {
    questionNum: 0,
    status: 0
  };
  yEvent = sessionStorage.getItem('event') ?? '';
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

  constructor(private route: ActivatedRoute, private router: Router, private dataService: DataService) { }

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

    connection.on("round1question", (data: any) => {
      this.loadQuestion(data);
    });

    connection.on("round1ShowAnswerChoices", (data: any) => {
      this.showChoices();
    });

    connection.on("round1CloseAnswer", (data: any) => {
      this.showAnswer();
    });

    connection.on("round1ShowMedia", (data: any) => {
      this.showMedia();
    });

    connection.on("round1intro", (data: any) => {
      this.changePage(data);
    });

    // set up our internal state
    this.dataService.getCurrentQuestion(this.yEvent).subscribe({next: (initialQ => {
      this.currentQuestion = initialQ;
    }), complete: () => {
      if (this.currentQuestion.questionNum > 0) {
        this.loadQuestion(this.currentQuestion.questionNum);

        if (this.currentQuestion.questionNum > 0 && this.currentQuestion.questionNum < 120)
        {
          // we have already loaded in the init so we have our state here. In theory.
          if (this.currentQuestion.status == 3)
          {
            this.showAnswer();
          }

          if (this.currentQuestion.status == 1)
          {
            this.showChoices();
          }
        };
      };
    }});
  }

  loadQuestion(questionId: number): void{
    this.currentQuestion = {
      questionNum: questionId,
      status: 0
    };
    this.questionVisible = true;
    this.answerVisible = false;
    this.answerShown = false;
    this.dataService.getRound1QuestionText(this.yEvent, questionId).pipe(takeUntil(this.destroy$))
        .subscribe(q => {
          this.currentQuestionDto = q;
          if (q.answerType == 1)
          {
            this.matchString = 'x' + this.convertStringToAnswer(q.correctAnswer);
          }
      });
      this.debugVals();
  };

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

  convertStringToAnswer(input: string):string {
    // shift all ascii bytes up by 16 to convert numbers to letters
    let newS = '';
    for (let i = 0; i < input.length; ++i) {
      // ASCII value
      let val = input[i].charCodeAt(0);
      newS += String.fromCharCode(val + 16);
    }
    return newS;
  }

  ngOnDestroy(): void {
  }

}
