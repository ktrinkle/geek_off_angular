import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { round1QADto, currentQuestionDto } from 'src/app/data/data';
import { DataService } from 'src/app/data.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { selectCurrentEvent, selectRound1AllQuestionsAndAnswers } from 'src/app/store';
import { round1AllQuestions } from 'src/app/store/round1/round1.actions';

@Component({
  selector: 'app-contestant',
  templateUrl: './contestant.component.html',
  styleUrls: ['./contestant.component.scss']
})
export class Round1ContestantComponent implements OnInit {
  // internal management since users won't leave this page
  hangTight: boolean = true;
  questionVisible: boolean = false;
  answerVisible: boolean = false;
  formVisible: boolean = false;
  answerSubmitted: boolean = false;
  answerReturn: string = '';
  multichoiceButton: any;
  currentQuestion: currentQuestionDto = {
    questionNum: 0,
    status: 0
  };
  allQuestionsAndAnwers: round1QADto[] = [];
  hideTime: Date = new Date;
  yEvent = '';
  public currentQuestionDto: round1QADto = {
    questionNum: 0,
    questionText: '',
    answers: [],
    answerType: -1,
    expireTime: new Date()
  };

  // define form
  public answerForm: FormGroup = new FormGroup({
    questionNum: new FormControl('', [Validators.pattern('[0-9]*')]),
    textAnswer: new FormControl(''),
    multipleChoice1: new FormControl(),
    multipleChoice2: new FormControl(),
    multipleChoice3: new FormControl(),
    multipleChoice4: new FormControl()
  });

  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private route: ActivatedRoute, private dataService: DataService, private store: Store) { }

  ngOnInit(): void {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.store.dispatch(round1AllQuestions({ yEvent: this.yEvent }));
      }
    });

    this.store.select(selectRound1AllQuestionsAndAnswers).pipe(takeUntil(this.destroy$)).subscribe(allQandA =>
      this.allQuestionsAndAnwers = allQandA as round1QADto[]
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
      this.getCurrentQuestion(data.questionNum);

      if (data.questionNum > 0 && data.questionNum < 120) {
        // we have already loaded in the init so we have our state here. In theory.
        if (data.status == 3) {
          this.closeForm();
        }

        if (data.status == 2) {
          this.openForm();
        }

        if (data.status == 1) {
          this.showChoices();
        }
      };

      if (data.questionNum == 0) {
        this.hangTight = true;
      }
    });
  }

  getCurrentQuestion(questionNum: number) {
    this.resetForm();

    this.hangTight = true;
    this.questionVisible = false;
    this.answerSubmitted = false;
    this.answerReturn = '';

    const currentQuestion = this.allQuestionsAndAnwers.filter(x => x.questionNum == questionNum);
    if (currentQuestion.length > 0) {
      this.currentQuestionDto = currentQuestion[0];
    }
  }

  showChoices() {
    this.hangTight = false;
    this.questionVisible = true;
    this.answerVisible = true;
    this.currentQuestion.status = 1;
    this.debugVals();
  }

  openForm() {
    this.hangTight = false;
    this.questionVisible = true;
    this.answerVisible = false;
    this.formVisible = true;
    this.currentQuestion.status = 2;
    this.debugVals();
  }

  closeForm() {
    this.formVisible = false;
    this.answerVisible = false;
    this.currentQuestion.status = 3;
    this.debugVals();
  }

  submitAnswer() {
    var answerText;
    var questionNum = this.currentQuestionDto?.questionNum ?? 0;

    if (this.currentQuestionDto?.answerType == 2) {
      // fill in process
      answerText = this.answerForm.value.textAnswer;
      console.log(answerText);
    }

    if (this.currentQuestionDto?.answerType == 0) {
      console.log(this.answerForm);
      // multiple guess process
      answerText = this.answerForm.value.textAnswer;
      console.log(answerText);
    }

    if (this.currentQuestionDto?.answerType == 1) {
      // match process
      answerText = this.answerForm.value.multipleChoice1.toString()
        + this.answerForm.value.multipleChoice2.toString()
        + this.answerForm.value.multipleChoice3.toString()
        + this.answerForm.value.multipleChoice4.toString();
    }
    if (questionNum > 0 && answerText != null) {
      this.dataService.saveRound1Answer(this.yEvent, questionNum, answerText).subscribe(a => {
        this.answerReturn = a;
        this.formVisible = false;
        this.answerSubmitted = true;
      });
    }
  }

  debugVals() {
    console.log("hangTight " + this.hangTight);
    console.log("questionVisible " + this.questionVisible);
    console.log("answerVisible " + this.answerVisible);
    console.log("formVisible " + this.formVisible);
    console.log("answerSubmitted " + this.answerSubmitted);
  }

  parseDtoToForm() {
    // convert DTO records to form. Rather brute force.
    var answerDto = this.currentQuestionDto?.answers;
    if (answerDto) {
      this.answerForm.patchValue({
        multipleChoice1: answerDto.find(a => { a.answerId == 1 })?.answer,
        multipleChoice2: answerDto.find(a => { a.answerId == 2 })?.answer,
        multipleChoice3: answerDto.find(a => { a.answerId == 3 })?.answer,
        multipleChoice4: answerDto.find(a => { a.answerId == 4 })?.answer
      });
    }
  }

  resetForm() {
    this.answerForm.patchValue({
      questionNum: '',
      textAnswer: '',
      multipleChoice1: '',
      multipleChoice2: '',
      multipleChoice3: '',
      multipleChoice4: ''
    })
  }



  /*
  * Future state:
  * 3a. Watch time based on time sent by API
  * 4. Pass answer back from UI
  * 5. Wait for next question to hit and do it all again, with internal state management
  * 7. Limit if question = 117, don't continue.
  */


}
