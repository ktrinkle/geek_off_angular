import { Component, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
import { round2SurveyQuestions, round2Answers } from '../../data/data';

@Component({
  selector: 'app-round2control',
  templateUrl: './round2control.component.html',
  styleUrls: ['./round2control.component.less']
})
export class Round2controlComponent implements OnInit {

  public yevent:string = 'e21';
  public surveyMasterList: round2SurveyQuestions[] = [];

  constructor(private _dataService: DataService) { }

  ngOnInit(): void {
    this.getSurveyQuestions(this.yevent);
  }

  getSurveyQuestions(yevent: string)
  {
      this._dataService.getAllRound2SurveyQuestion(yevent).subscribe((data: round2SurveyQuestions[]) => {
        this.surveyMasterList = data;
      });
      console.log(this.surveyMasterList);
  }

}
