import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { round1QADto } from 'src/app/data/data';
import { DataService } from 'src/app/data.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';


@Component({
  selector: 'app-contestant',
  templateUrl: './contestant.component.html',
  styleUrls: ['./contestant.component.scss']
})
export class Round1ContestantComponent implements OnInit {
  // internal management since users won't leave this page
  questionVisible:boolean = false;
  answerVisible: boolean = false;
  answerSubmitted: boolean = false;
  currentQuestion: number = 0;
  hideTime: Date = new Date;
  yEvent = 'e21';
  currentQuestionDto:round1QADto[] = [];

  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private route: ActivatedRoute, private dataService: DataService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.currentQuestion = params['question'];
      console.log(this.currentQuestion);
    });

    if (this.currentQuestion > 0)
    {
      // call dataservice
    }
  }

  /*
  * Future state:
  * 1. Get question based on param, or leave as blank if none
  * 2. Listen for signal from controller
  * 3. If signal received, accept it and display question...or answer
  * 3a. Watch time based on time sent by API
  * 4. Pass answer back from UI
  * 5. Wait for next question to hit and do it all again, with internal state management
  * 6. Verify current question just in case of reload...we should have this, or say screw the router?
  * 7. Limit if question = 117, don't continue.
  */

  getCurrentQuestionText(): void {

    if (this.currentQuestion < 100 || this.currentQuestion > 199)
    {
      this.currentQuestionDto = [];
    }

    this.dataService.getRound1QuestionText(this.yEvent, this.currentQuestion).pipe(takeUntil(this.destroy$))
        .subscribe(q => {
          this.currentQuestionDto = q;
      });
  }

  getCurrentQuestionAnswer(): void {

    if (this.currentQuestion < 100 || this.currentQuestion > 199)
    {
      this.currentQuestionDto = [];
    }

    this.dataService.getRound1QuestionAnswer(this.yEvent, this.currentQuestion).pipe(takeUntil(this.destroy$))
        .subscribe(q => {
          this.currentQuestionDto = q;
      });
  }

}
