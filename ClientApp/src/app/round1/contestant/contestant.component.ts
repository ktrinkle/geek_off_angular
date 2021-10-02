import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { round1QADto, currentQuestionDto } from 'src/app/data/data';
import { DataService } from 'src/app/data.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';



@Component({
  selector: 'app-contestant',
  templateUrl: './contestant.component.html',
  styleUrls: ['./contestant.component.scss']
})
export class Round1ContestantComponent implements OnInit {
  // internal management since users won't leave this page
  questionVisible:boolean = false;
  answerVisible: boolean = false;
  formVisible: boolean = false;
  answerSubmitted: boolean = false;
  currentQuestion: currentQuestionDto = {
    questionNum: 0,
    status: 0
  };
  hideTime: Date = new Date;
  yEvent = sessionStorage.getItem('event') ?? '';
  currentQuestionDto:round1QADto | undefined;

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

  constructor(private route: ActivatedRoute, private dataService: DataService) {
    this.dataService.getCurrentQuestion(this.yEvent).subscribe(initialQ => {
      this.currentQuestion = initialQ;
    });
  }

  ngOnInit(): void {

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + 'event')
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

    connection.on("round1OpenAnswer", (data: any) => {
      this.openForm();
    });

    connection.on("round1CloseAnswer", (data: any) => {
      this.closeForm();
    });

    // set up our internal state
    if (this.currentQuestion.questionNum > 0 && this.currentQuestion.questionNum < 120)
    {
      this.loadQuestion(this.currentQuestion.questionNum);

      if (this.currentQuestion.status == 3)
      {
        this.closeForm();
      }

      if (this.currentQuestion.status == 2)
      {
        this.openForm();
      }

      if (this.currentQuestion.status == 1)
      {
        this.showChoices();
      }
    }
  }

  loadQuestion(questionId: number): void{
    this.currentQuestion = {
      questionNum: questionId,
      status: 0
    };
    this.questionVisible = false;
    this.answerSubmitted = false;
    this.dataService.getRound1QuestionAnswer(this.yEvent, questionId).pipe(takeUntil(this.destroy$))
        .subscribe(q => {
          this.currentQuestionDto = q;
      });
  };

  showChoices() {
    this.questionVisible = true;
    this.answerVisible = true;
    this.currentQuestion.status = 1;
  }

  openForm() {
    this.formVisible = true;
    this.currentQuestion.status = 2;
  }

  closeForm() {
    this.formVisible = false;
    this.answerVisible = false;
    this.currentQuestion.status = 3;
  }

  submitAnswer() {
    // future to code
  }



  /*
  * Future state:
  * 3a. Watch time based on time sent by API
  * 4. Pass answer back from UI
  * 5. Wait for next question to hit and do it all again, with internal state management
  * 7. Limit if question = 117, don't continue.
  */


}
