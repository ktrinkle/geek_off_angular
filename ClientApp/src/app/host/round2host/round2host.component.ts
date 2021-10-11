import { round2SurveyList } from './../../data/data';
import { Component, OnDestroy, OnInit } from '@angular/core';
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

  public yevent: string = sessionStorage.getItem('event') ?? '';
  public surveyMasterList: round2SurveyList[] = [];

  constructor(private _dataService: DataService,
    private formBuilder: FormBuilder) { }


  ngOnInit(): void {
  // Grabs all the questions.
    this._dataService.getAllRound2SurveyQuestions(this.yevent).subscribe((data: round2SurveyList[]) => {
      this.surveyMasterList = data;
    });


  }
}
