import { Component, OnDestroy, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
import { Store } from '@ngrx/store';
import { round2AllSurvey } from '../../store/round2/round2.actions';
import { round2SurveyQuestions, round2SubmitAnswer, round2Answers, round23Scores } from '../../data/data';
import { UntypedFormGroup, UntypedFormControl, Validators, UntypedFormBuilder, UntypedFormArray } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { Round2countdowndialogComponent } from 'src/app/round2/round2countdowndialog/round2countdowndialog.component';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { selectCurrentEvent } from 'src/app/store';

@Component({
  selector: 'app-round2control',
  templateUrl: './round2control.component.html',
  styleUrls: ['./round2control.component.scss']
})
export class Round2controlComponent implements OnInit, OnDestroy {
  public yEvent = '';
  public surveyMasterList: round2SurveyQuestions[] = [];
  public newEventForm: UntypedFormGroup = new UntypedFormGroup({
    teamNum: new UntypedFormControl('', [Validators.pattern('[0-9]*')]),
    playerNum: new UntypedFormControl('', [Validators.pattern('1|2')]),
    questions: new UntypedFormArray([]),
  });
  public apiResponse: string = '';
  public showBonusQuestion: boolean = false;
  public scoreboard: round23Scores[] = [];
  public currentDisplayId: number = 0;
  public firstPlayerAnswers: round2Answers[] = [];
  buzzer = new Audio();
  dings = new Audio();
  consolation = new Audio('https://geekoff2021static.blob.core.windows.net/snd/r2_consolation.m4a');
  destroy$: Subject<boolean> = new Subject<boolean>();
  countdownValue: number = 0;   // Countdown timer duration.

  public pickAnimateForm: UntypedFormGroup = new UntypedFormGroup({
    currentDisplayId: new UntypedFormControl(this.currentDisplayId)
  })

  displayList: any[] = [
    { key: 0, value: 'Hide all' },
    { key: 1, value: 'Player 1 Answer 1' },
    { key: 2, value: 'Player 1 Score 1' },
    { key: 3, value: 'Player 1 Answer 2' },
    { key: 4, value: 'Player 1 Score 2' },
    { key: 5, value: 'Player 1 Answer 3' },
    { key: 6, value: 'Player 1 Score 3' },
    { key: 7, value: 'Player 1 Answer 4' },
    { key: 8, value: 'Player 1 Score 4' },
    { key: 9, value: 'Player 1 Answer 5' },
    { key: 10, value: 'Player 1 Score 5' },
    { key: 11, value: 'Player 2 Answer 1' },
    { key: 12, value: 'Player 2 Score 1' },
    { key: 13, value: 'Player 2 Answer 2' },
    { key: 14, value: 'Player 2 Score 2' },
    { key: 15, value: 'Player 2 Answer 3' },
    { key: 16, value: 'Player 2 Score 3' },
    { key: 17, value: 'Player 2 Answer 4' },
    { key: 18, value: 'Player 2 Score 4' },
    { key: 19, value: 'Player 2 Answer 5' },
    { key: 20, value: 'Player 2 Score 5' },
  ]

  playerNumbers: number[] = [
    1,
    2
  ];

  finalizeState: string = 'Finalize Round';

  constructor(private store: Store, private _dataService: DataService,
    private formBuilder: UntypedFormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        // this.store.dispatch(round2AllSurvey({ yEvent: this.yEvent }));
        this.getSurveyQuestions(this.yEvent);
        this.updateScoreboard();
      }
    });

    this.buzzer.src = 'https://geekoff2021static.blob.core.windows.net/snd/feud-fm-buzzer.mp3';
    this.dings.src = 'https://geekoff2021static.blob.core.windows.net/snd/feud-fm-dings.mp3';
    this.buzzer.load();
    this.dings.load();
    this.consolation.load();

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

    connection.on("round2ScoreUpdate", (data: any) => {
      this.updateScoreboard();
    })

    connection.on("round2ChangeTeam", (data: any) => {
      console.log(data);
    })

    connection.on("round2Answer", (data: any) => { })

    connection.on("round2AnswerShow", (data: any) => { })

  }

  // Grabs all the questions.
  getSurveyQuestions(yevent: string) {
    this._dataService.getAllRound2FeudSurveyQuestions(yevent).subscribe((data: round2SurveyQuestions[]) => {
      this.surveyMasterList = data;

      // creates the formArray of the formGroup of question controls
      const qArray = this.formBuilder.array([]);
      for (let question of this.surveyMasterList) {
        qArray.push(
          this.formBuilder.group({
            questionNum: new UntypedFormControl(question.questionNum),
            answer: new UntypedFormControl(''),
            score: new UntypedFormControl('', [Validators.pattern('[0-9]*')])
          })
        );
      }

      // creates the entire form
      this.newEventForm = new UntypedFormGroup({
        teamNum: new UntypedFormControl('', [Validators.pattern('[0-9]*')]),
        playerNum: new UntypedFormControl('', [Validators.pattern('1|2')]),
        questions: qArray,
      });

      console.log(this.newEventForm);
    });
  }

  getQuestions(form: any) {
    return form?.get('questions').controls;
  }

  getAnswers(questionNumber: any) {
    const question = this.surveyMasterList.filter(x => x.questionNum == questionNumber);
    if (question.length > 0) {
      return question[0].surveyAnswers;
    }
    return [];
  }

  // Get user answer and store in DB (see)
  saveUserAnswer(questionGroup: any) {
    var submitAnswer: round2SubmitAnswer = {
      yEvent: this.yEvent,
      questionNum: questionGroup.get('questionNum').value,
      playerNum: this.newEventForm.get('playerNum')?.value,
      teamNum: this.newEventForm.get('teamNum')?.value,
      answer: questionGroup.get('answer').value,
      score: questionGroup.get('score').value
    };

    console.log(submitAnswer);

    this._dataService.sendRound2FeudAnswerText(submitAnswer).subscribe((data: string) => {
      this.apiResponse = data;
    });

    questionGroup.get('answer').reset();
    questionGroup.get('score').reset();
    // this.newEventForm.get("questionNum")?.reset();
  }

  toggleBonusQuestion() {
    this.showBonusQuestion = !this.showBonusQuestion;
    if (this.showBonusQuestion) {
      console.log("Showing bonus...")
    }
    else {
      console.log("Hiding bonus...");
    }
  }

  presetAnswer(answer: any, question: UntypedFormGroup) {
    question.get("answer")?.setValue(answer.answer);
    question.get("score")?.setValue(answer.score);
    question.get("questionNum")?.setValue(answer.questionNum);
  }

  async updateRemoteScoreboard() {
    await this._dataService.updateScoreboardRound2Feud();
    this.updateScoreboard();
  }

  updateScoreboard() {
    this._dataService.getRound2FeudScores(this.yEvent).subscribe({
      next: (a => {
        this.scoreboard = a;
      }), complete: () => {
        this.scoreboard.sort((a, b) => 0 - ((a.teamScore ?? 0) > (b.teamScore ?? 0) ? 1 : -1));
      }
    });
  }

  // may need to move this to howler
  playBuzzer() {
    this.buzzer.play().then(_b => {
      this.buzzer.currentTime = 0;
      this.buzzer.pause;
    });
  }

  playDings() {
    this.dings.play().then(_b => {
      this.dings.currentTime = 0;
      this.dings.pause;
    });
  }

  showDisplayBoardValue() {
    // this will be values 0-20. 0 = hide all, 10 = show all player 1, 20 = show all
    var entryNum = this.pickAnimateForm.value.currentDisplayId;
    this._dataService.revealRound2FeudValue(entryNum);
  }

  showFirstAnswers(form: UntypedFormGroup) {
    if (this.newEventForm.get("playerNum")?.value == "2") {
      this._dataService.getRound2FeudFirstPlayer(this.yEvent, this.newEventForm.get("teamNum")?.value)
        .subscribe((data: round2Answers[]) => this.firstPlayerAnswers = data);
      console.log("FPA: " + this.firstPlayerAnswers);
    }
  }

  changeTeamPlayer() {
    var teamNum = this.newEventForm.get("teamNum")?.value;
    console.log('Got team number ' + teamNum);

    if (teamNum != '--')
    {
      this._dataService.changeRound2FeudTeam(teamNum);
      this.newEventForm.patchValue({ "playerNum": "1" });
    }
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(Round2countdowndialogComponent, {
      width: '250px',
      data: { countdownValue: this.countdownValue }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The Dialog was closed');
      this.countdownValue = result;
      this.setCountdown(result);
    });
  }

  startCountdown() {
    this._dataService.startCountdown(this.countdownValue);
  }

  stopCountdown() {
    this._dataService.stopCountdown();
  }

  setCountdown(seconds: number) {
    this._dataService.setCountdown(Number(seconds));
  }

  changeScreen(name: string) {
    this._dataService.changeRound2FeudPage(name);
    if (name == 'prize1') {
      this.consolation.play();
    }
  }

  finalizeRound() {
    this._dataService.finalizeRound2Feud(this.yEvent).subscribe(data => {
      this.finalizeState = data;
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
