import { Component, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
// import { round2SurveyQuestions, round2Answers } from '../../data/data';
import { round2SurveyQuestions} from '../../data/data';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-round2host',
  templateUrl: './round2host.component.html',
  styleUrls: ['./round2host.component.scss']
})

export class Round2hostComponent implements OnInit {

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
      playerNum: new FormControl('', [Validators.pattern('1|2')])
      // answer: new FormControl(''),
      // score: new FormControl('', [Validators.pattern('[0-9]*')])
    });
  }

  // Grabs all the questions.
  getSurveyQuestions(yevent: string) {
    this._dataService.getAllRound2SurveyQuestions(yevent).subscribe((data: round2SurveyQuestions[]) => {
      this.surveyMasterList = data;
    });
    console.log(this.surveyMasterList);
  }


}
