import { Component, OnDestroy, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
import { Store } from '@ngrx/store';
import { round3AnswerDto, round3QuestionDto, round23Scores, introDto } from '../../data/data';
import { FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { selectCurrentEvent } from 'src/app/store';

@Component({
  selector: 'app-round3control',
  templateUrl: './round3control.component.html',
  styleUrls: ['./round3control.component.scss']
})
export class Round3controlComponent implements OnInit, OnDestroy {
  public yEvent = '';
  public round3Questions: round3QuestionDto[] = [];
  public round3Form: FormGroup = new FormGroup({
    teams: new FormArray([]),
    questionNum: new FormControl(''),
  });
  public apiResponse: string = '';
  public scoreboard: round23Scores[] = [];
  public teamList: introDto[] = [];
  public currentDisplayId: number = 0;
  destroy$: Subject<boolean> = new Subject<boolean>();

  finalizeState: string = 'Finalize Round';

  public colors: string[] = [
    'red',
    'green',
    'blue',
  ]

  constructor(private store: Store, private _dataService: DataService,
    private formBuilder: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this._dataService.getAllRound3Teams(this.yEvent).subscribe(t => {
          this.teamList = t;
          this.getRound3Questions(this.yEvent);
        })
        this.updateScoreboard();
      }
    });

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Debug)
      .withUrl(environment.api_url + '/events')
      .withAutomaticReconnect()
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round3ScoreUpdate", (data: any) => {
      this.updateScoreboard();
    })

  }

  getRound3Questions(yevent: string) {
    this._dataService.getAllRound3Questions(yevent).subscribe((data: round3QuestionDto[]) => {
      this.round3Questions = data;

      // creates the formArray of the formGroup of question controls
      const tArray = this.formBuilder.array([]);
      for (let team of this.teamList) {
        tArray.push(
          this.formBuilder.group({
            teamNum: new FormControl(team.teamNo),
            teamName: new FormControl(team.teamName),
            score: new FormControl('', [Validators.pattern('[0-9]*')])
          })
        );
      }

      // creates the entire form
      this.round3Form = new FormGroup({
        teams: tArray,
        questionNum: new FormControl('', [Validators.pattern('[0-9]*')])
      });
    });
  }

  getQuestions(form: any) {
    return form?.get('questions').controls;
  }

  getScore(questionNumber: any) {
    const question = this.round3Questions.find(x => x.questionNum == questionNumber);
    return question?.score;
  }

  async updateRemoteScoreboard() {
    await this._dataService.updateScoreboardRound3();
    this.updateScoreboard();
  }

  updateScoreboard() {
    this._dataService.getRound3Scores(this.yEvent).subscribe(a => {
        this.scoreboard = a;
      });
  }

  // Get user answer and store in DB (see)
  saveUserAnswer() {
   /*var submitAnswer: round3AnswerDto = {
      yEvent: this.yEvent,
      questionNum: questionGroup.get('questionNum').value,
      playerNum: this.newEventForm.get('playerNum')?.value,
      teamNum: this.newEventForm.get('teamNum')?.value,
      answer: questionGroup.get('answer').value,
      score: questionGroup.get('score').value
    };

    console.log(submitAnswer);

    this._dataService.sendRound2AnswerText(submitAnswer).subscribe((data: string) => {
      this.apiResponse = data;
    });

    questionGroup.get('answer').reset();
    questionGroup.get('score').reset();
    // this.newEventForm.get("questionNum")?.reset();*/
  }

  finalizeRound() {
    this._dataService.finalizeRound3(this.yEvent).subscribe(data => {
      this.finalizeState = data;
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

}
