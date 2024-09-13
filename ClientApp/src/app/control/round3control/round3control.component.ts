import { Component, OnDestroy, OnInit, Pipe, PipeTransform } from '@angular/core';
import { DataService } from '../../data.service';
import { Store } from '@ngrx/store';
import { round3AnswerDto, round3QuestionDto, round23Scores, introDto } from '../../data/data';
import { UntypedFormGroup, UntypedFormControl, Validators, UntypedFormBuilder, UntypedFormArray } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { selectCurrentEvent, selectRound3Scores } from 'src/app/store';
import { round3Score } from 'src/app/store/round3/round3.actions';

@Pipe({ name: 'displayTeamInfo' })
export class DisplayTeamInfoPipe implements PipeTransform {
  transform(teamList: introDto[], teamNumber: number): string {
    const team = teamList.filter(x => x.teamNum === teamNumber);
    if (team.length === 0) {
      return '';
    }
    return `${team[0].teamNum} - ${team[0].teamName}`;
  }
}

@Component({
  selector: 'app-round3control',
  templateUrl: './round3control.component.html',
  styleUrls: ['./round3control.component.scss']
})
export class Round3controlComponent implements OnInit, OnDestroy {
  public yEvent = '';
  public round3Questions: round3QuestionDto[] = [];
  public round3Form: UntypedFormGroup = new UntypedFormGroup({
    teams: new UntypedFormArray([]),
    questionNum: new UntypedFormControl(''),
  });

  public finalJepForm: UntypedFormGroup = new UntypedFormGroup({
    teams: new UntypedFormArray([]),
    questionNum: new UntypedFormControl('350'),
  });

  public selectedScore = 0;
  public apiResponse = '';
  public scoreboard: round23Scores[] = [];
  public teamList: introDto[] = [];
  public selectedQuestion = 0;

  destroy$: Subject<boolean> = new Subject<boolean>();

  finalizeState = 'Finalize Round';

  public colors: string[] = [
    'red',
    'green',
    'blue',
  ]

  constructor(private store: Store, private _dataService: DataService,
    private formBuilder: UntypedFormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this._dataService.getAllRound3Teams(this.yEvent).subscribe(t => {
          this.teamList = t;

          console.log('calling store dispatch', this.yEvent);
          this.store.dispatch(round3Score({ yEvent: this.yEvent }));

          console.log('calling getround3questions');
          this.getRound3Questions(this.yEvent);
        })
        this.updateScoreboard();
      }

    });

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Debug)
      .withUrl(environment.api_url + '/events', { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round3ScoreUpdate", () => {
      this.updateScoreboard();
    })

  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  getTeamsFromForm(form: any) {
    return form?.get('teams')?.controls;
  }

  getRound3Questions(yevent: string) {
    this._dataService.getAllRound3Questions(yevent).subscribe((data: round3QuestionDto[]) => {
      this.round3Questions = data;

      // // mock data for testing
      // this.round3Questions = [];
      // for (let i = 0; i < 10; i++) {
      //   this.round3Questions.push({
      //     questionNum: i,
      //     sortOrder: i,
      //     score: i * 100
      //   });
      // }

      // creates the formArray of the formGroup of question controls
      const tArray = this.formBuilder.array([]);
      for (const team of this.teamList) {
        tArray.push(
          this.formBuilder.group({
            teamNum: new UntypedFormControl(team.teamNum),
            score: new UntypedFormControl('', [Validators.pattern('[0-9]*')])
          })
        );
      }

      // creates the entire form
      this.round3Form = new UntypedFormGroup({
        teams: tArray,
        questionNum: new UntypedFormControl('', [Validators.pattern('[0-9]*')])
      });

      this.finalJepForm = new UntypedFormGroup({
        teams: tArray,
        questionNum: new UntypedFormControl('350')
      });
    });
  }

  getScore(questionNumber: number): void {
    this.resetSelections(this.round3Form);
    const question = this.round3Questions.find(x => x.questionNum == questionNumber);
    this.selectedScore = question?.score ?? 0;
    this.selectedQuestion = questionNumber;
  }

  async updateRemoteScoreboard() {
    await this._dataService.updateScoreboardRound3();
    this.updateScoreboard();
  }

  updateScoreboard() {
    this.store.select(selectRound3Scores).pipe(takeUntil(this.destroy$)).subscribe((data: round23Scores[]) => {
      this.scoreboard = data;
    });
  }

  resetSelections(form: UntypedFormGroup) {
    const teamFormArray = form.get('teams') as UntypedFormArray;
    for (const team of teamFormArray.controls) {
      team.get('score')?.reset();
    }
    this.selectedScore = 0;
  }

  // Get user answer and store in DB
  saveUserAnswer(form: UntypedFormGroup) {
    const submitArray = [];
    const teamFormArray = form?.get('teams') as UntypedFormArray;
    for (const team of teamFormArray.controls) {
      const teamScore: round3AnswerDto = {
        yEvent: this.yEvent,
        questionNum: form.get('questionNum')?.value as number,
        teamNum: team.get('teamNum')?.value,
        score: team.get('score')?.value as number ?? 0
      };
      submitArray.push(teamScore);
    }
    const questionNum = form.get('questionNum')?.value as number;

    console.log(submitArray);

    this._dataService.updateRound3Scores(submitArray).subscribe((data: string) => {
      this.apiResponse = data;
      this.store.dispatch(round3Score({ yEvent: this.yEvent}));
      this.updateRemoteScoreboard();
    });

    this.resetSelections(form);
    form.get('questionNum')?.reset();
    this.selectedQuestion = 0;

    const idx = this.round3Questions.findIndex(q => questionNum == q.questionNum);
    console.log('question search', idx);
    if (idx >= -1) {
      this.round3Questions[idx].disabled = true;
    }
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
