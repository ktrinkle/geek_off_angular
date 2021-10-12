import { Component, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
import { Store } from '@ngrx/store';
import { round2AllSurvey } from '../../store/round2/round2.actions';
import { round2SurveyQuestions, round2SubmitAnswer, round2Answers } from '../../data/data';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-round2control',
  templateUrl: './round2control.component.html',
  styleUrls: ['./round2control.component.scss']
})
export class Round2controlComponent implements OnInit {

  public yevent = sessionStorage.getItem('event') ?? '';
  public surveyMasterList: round2SurveyQuestions[] = [];
  public newEventForm: FormGroup = new FormGroup({});
  public questionNum: number = 0;
  public teamNum: number = 0;
  public playerNum: number = 0;
  public answer: string = '';
  public score: number = 0;
  public apiResponse: string = '';
  public showBonusQuestion: boolean = false;
  constructor(private store: Store, private _dataService: DataService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.store.dispatch(round2AllSurvey({ yEvent: 'e21' }));

    this.getSurveyQuestions(this.yevent);

    this.newEventForm = new FormGroup({
      teamNum: new FormControl('', [Validators.pattern('[0-9]*')]),
      playerNum: new FormControl('', [Validators.pattern('1|2')]),
      answer: new FormControl(''),
      score: new FormControl('', [Validators.pattern('[0-9]*')])
    });
  }

  // Grabs all the questions.
  getSurveyQuestions(yevent: string) {
    this._dataService.getAllRound2SurveyQuestions(yevent).subscribe((data: round2SurveyQuestions[]) => {
      this.surveyMasterList = data;
      console.log(this.surveyMasterList);
    });
    console.log(this.surveyMasterList);
  }

  // Get user answer and store in DB (see)
  saveUserAnswer(question: any) {
    console.log(this.newEventForm);

    var submitAnswer: round2SubmitAnswer = {
      yEvent: this.yevent,
      questionNum: question.questionNum,
      playerNum: this.newEventForm.value.playerNum,
      teamNum: this.newEventForm.value.teamNum,
      answer: this.newEventForm.value.answer,
      score: this.newEventForm.value.score
    };

    console.log(submitAnswer);

    this._dataService.sendRound2AnswerText(submitAnswer).subscribe((data: string) => {
                                              this.apiResponse = data;
    });

    this.newEventForm.get("answer")?.reset();
    this.newEventForm.get("score")?.reset();
    this.newEventForm.get("questionNum")?.reset();

  }

  toggleBonusQuestion() {
    this.showBonusQuestion = !this.showBonusQuestion;
    if(this.showBonusQuestion) {
      console.log("Showing bonus...")
    }
    else {
      console.log("Hiding bonus...");
    }
  }

  presetAnswer(answer: round2Answers) {
    this.newEventForm.get("answer")?.setValue(answer.answer);
    this.newEventForm.get("score")?.setValue(answer.score);
    this.newEventForm.get("questionNum")?.setValue(answer.questionNum);
  }
}
