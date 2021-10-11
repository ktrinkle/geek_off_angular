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
  hangTight:boolean = true;
  questionVisible:boolean = false;
  answerVisible: boolean = false;
  formVisible: boolean = false;
  answerSubmitted: boolean = false;
  answerReturn: string = '';
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

  constructor(private route: ActivatedRoute, private dataService: DataService) { }

  ngOnInit(): void {

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events')
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

    connection.on("round1PlayerAnswer", (data: any) => {
      this.closeForm();
    });

    connection.on("round1CloseAnswer", (data: any) => {
      this.closeForm();
    });

    // set up our internal state
    // this is not working as we desire on reloads of the component, need to review
    this.dataService.getCurrentQuestion(this.yEvent).subscribe({next: (initialQ => {
      this.currentQuestion = initialQ;
    }), complete: () => {
      if (this.currentQuestion.questionNum > 0) {
        this.loadQuestion(this.currentQuestion.questionNum);

        console.log(this.currentQuestion);
        if (this.currentQuestion.questionNum > 0 && this.currentQuestion.questionNum < 120)
        {
          // we have already loaded in the init so we have our state here. In theory.
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
        };

        if (this.currentQuestion.questionNum == 0)
        {
          this.hangTight = true;
        }

      };
    }});
  }

  loadQuestion(questionId: number): void{
    this.currentQuestion = {
      questionNum: questionId,
      status: 0
    };
    this.hangTight = true;
    this.questionVisible = false;
    this.answerSubmitted = false;
    this.answerReturn = '';
    this.dataService.getRound1QuestionAnswer(this.yEvent, questionId).pipe(takeUntil(this.destroy$))
        .subscribe(q => {
          this.resetForm(); // confirm this does what we expect.
          this.currentQuestionDto = q;
          // inject form
          this.answerForm.patchValue({
            questionNum: q.questionId
          });

          // run this if we have a multiple choice or match (0/1)
          if (q.questionAnswerType < 2) {
            this.parseDtoToForm();
          }
          console.log(this.answerForm);
      });
      this.debugVals();
  };

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

    if (this.currentQuestionDto?.answerType != 1)
    {
      // multiple guess and fill in process
      answerText = this.answerForm.value.textAnswer;
      console.log(answerText);
    }

    if (this.currentQuestionDto?.answerType == 1)
    {
      // match process
      answerText = this.answerForm.value.multipleChoice1.toString()
                    + this.answerForm.value.multipleChoice2.toString()
                    + this.answerForm.value.multipleChoice3.toString()
                    + this.answerForm.value.multipleChoice4.toString();
    }
    if (questionNum > 0 && answerText != null)
    {
      this.dataService.saveRound1Answer(this.yEvent, questionNum, answerText).subscribe(a => {
        this.answerReturn = a;
        this.formVisible = false;
      });
    }
  }

  debugVals() {
    console.log("hangTight " + this.hangTight);
    console.log("questionVisible " + this.questionVisible);
    console.log("answerVisible " + this.answerVisible);
    console.log("formVisible" + this.formVisible);
  }

  parseDtoToForm() {
    // convert DTO records to form. Rather brute force.
    var answerDto = this.currentQuestionDto?.answers;
    if (answerDto) {
      this.answerForm.patchValue({
        multipleChoice1: answerDto.find(a => {a.answerId == 1})?.answer,
        multipleChoice2: answerDto.find(a => {a.answerId == 2})?.answer,
        multipleChoice3: answerDto.find(a => {a.answerId == 3})?.answer,
        multipleChoice4: answerDto.find(a => {a.answerId == 4})?.answer
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
