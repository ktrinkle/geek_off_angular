import { Component, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
import { round2SurveyQuestions, round2SubmitAnswer } from '../../data/data';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-round2control',
  templateUrl: './round2control.component.html',
  styleUrls: ['./round2control.component.less']
})
export class Round2controlComponent implements OnInit {

  public yevent: string = 'e21';
  public surveyMasterList: round2SurveyQuestions[] = [];
  public newEventForm: FormGroup = new FormGroup({});
  public questionNum: number = 0;
  public teamNum: number = 0;
  public playerNum: number = 0;
  public answer: string = '';
  public score: number = 0;
  public apiRespone: string = '';

  constructor(private _dataService: DataService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.getSurveyQuestions(this.yevent);

    this.newEventForm = new FormGroup({
      questionNum: new FormControl('', [Validators.pattern('[0-9]*')]),
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
    });
    console.log(this.surveyMasterList);
  }

  // Get user answer and store in DB (see)
  saveUserAnswer() {
    console.log(this.newEventForm);

    var submitAnswer: round2SubmitAnswer = {
      yEvent: this.yevent,
      questionNum: this.newEventForm.value.questionNum,
      playerNum: this.newEventForm.value.playerNum,
      teamNum: this.newEventForm.value.teamNum,
      answer: this.newEventForm.value.answer,
      score: this.newEventForm.value.score
    };

    console.log(submitAnswer);

    this._dataService.sendRound2AnswerText(submitAnswer).subscribe((data: string) => {
                                              this.apiRespone = data;
    });
  }
}
