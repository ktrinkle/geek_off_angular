import { Component, OnInit, Inject } from '@angular/core';
import { DataService } from '../../data.service';
import { Store } from '@ngrx/store';
import { round2AllSurvey } from '../../store/round2/round2.actions';
import { round2SurveyQuestions, round2SubmitAnswer, round2Answers, round23Scores } from '../../data/data';
import { FormGroup, FormControl, Validators, FormBuilder, Form } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Round2countdowndialogComponent } from 'src/app/round2/round2countdowndialog/round2countdowndialog.component';
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
  public scoreboard: round23Scores[] = [];
  public currentDisplayId: number = 0;
  public firstPlayerAnswers: round2Answers[] = [];
  buzzer = new Audio();
  dings = new Audio();
  consolation = new Audio('https://geekoff2021static.blob.core.windows.net/snd/r2_consolation.m4a');

  countdownValue: number = 0;   // Countdown timer duration.

  public pickAnimateForm: FormGroup = new FormGroup({
    currentDisplayId: new FormControl(this.currentDisplayId)
  })

  displayList: any[] = [
    {key: 0, value: 'Hide all'},
    {key: 1, value: 'Player 1 Answer 1'},
    {key: 2, value: 'Player 1 Score 1'},
    {key: 3, value: 'Player 1 Answer 2'},
    {key: 4, value: 'Player 1 Score 2'},
    {key: 5, value: 'Player 1 Answer 3'},
    {key: 6, value: 'Player 1 Score 3'},
    {key: 7, value: 'Player 1 Answer 4'},
    {key: 8, value: 'Player 1 Score 4'},
    {key: 9, value: 'Player 1 Answer 5'},
    {key: 10, value: 'Player 1 Score 5'},
    {key: 11, value: 'Player 2 Answer 1'},
    {key: 12, value: 'Player 2 Score 1'},
    {key: 13, value: 'Player 2 Answer 2'},
    {key: 14, value: 'Player 2 Score 2'},
    {key: 15, value: 'Player 2 Answer 3'},
    {key: 16, value: 'Player 2 Score 3'},
    {key: 17, value: 'Player 2 Answer 4'},
    {key: 18, value: 'Player 2 Score 4'},
    {key: 19, value: 'Player 2 Answer 5'},
    {key: 20, value: 'Player 2 Score 5'},
  ]

  constructor(private store: Store, private _dataService: DataService,
    private formBuilder: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.store.dispatch(round2AllSurvey({ yEvent: this.yevent }));
    this.buzzer.src = 'https://geekoff2021static.blob.core.windows.net/snd/feud-fm-buzzer.mp3';
    this.dings.src = 'https://geekoff2021static.blob.core.windows.net/snd/feud-fm-dings.mp3';
    this.buzzer.load();
    this.dings.load();
    this.consolation.load();

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
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

    this.getSurveyQuestions(this.yevent);
    this.updateScoreboard();

    this.newEventForm = new FormGroup({
      teamNum: new FormControl('', [Validators.pattern('[0-9]*')]),
      playerNum: new FormControl('', [Validators.pattern('1|2')]),
    });
  }

  // Grabs all the questions.
  getSurveyQuestions(yevent: string) {
    this._dataService.getAllRound2SurveyQuestions(yevent).subscribe((data: round2SurveyQuestions[]) => {
      this.surveyMasterList = data;
      this.surveyMasterList.forEach(question => {
        var answerControl = `answer${question.questionNum}`;
        var scoreControl = `score${question.questionNum}`;

        console.log(`HERE: ${answerControl}, ${scoreControl}`)

        this.newEventForm.addControl(answerControl, new FormControl(''));
        this.newEventForm.addControl(scoreControl, new FormControl('', [Validators.pattern('[0-9]*')]));
      });
    });
  }

  // Get user answer and store in DB (see)
  saveUserAnswer(question: any) {
    console.log(this.newEventForm);

    var submitAnswer: round2SubmitAnswer = {
      yEvent: this.yevent,
      questionNum: question.questionNum,
      playerNum: this.newEventForm.value.playerNum,
      teamNum: this.newEventForm.value.teamNum,
      answer: this.newEventForm.get(`answer${question.questionNum}`)?.value,
      score: this.newEventForm.get(`score${question.questionNum}`)?.value
    };

    console.log(submitAnswer);

    this._dataService.sendRound2AnswerText(submitAnswer).subscribe((data: string) => {
                                              this.apiResponse = data;
    });

    this.newEventForm.get(`answer${question.questionNum}`)?.reset();
    this.newEventForm.get(`score${question.questionNum}`)?.reset();
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
    this.newEventForm.get(`answer${answer.questionNum}`)?.setValue(answer.answer);
    this.newEventForm.get(`score${answer.questionNum}`)?.setValue(answer.score);
    this.newEventForm.get("questionNum")?.setValue(answer.questionNum);
  }

  async updateRemoteScoreboard()
  {
    await this._dataService.updateScoreboardRound2();
    this.updateScoreboard();
  }

  updateScoreboard()
  {
    this._dataService.getRound2Scores(this.yevent).subscribe({next: (a => {
      this.scoreboard = a;
    }), complete: () => {
      this.scoreboard.sort((a,b) => 0 - ((a.teamScore ?? 0) > (b.teamScore ?? 0) ? 1 : -1));
    }
  });
  }

  // may need to move this to howler
  playBuzzer() {
    this.buzzer.play().then( _b => {
      this.buzzer.currentTime = 0;
      this.buzzer.pause;
    });
  }

  playDings() {
    this.dings.play().then( _b => {
      this.dings.currentTime = 0;
      this.dings.pause;
    });
  }

  showDisplayBoardValue(){
    // this will be values 0-20. 0 = hide all, 10 = show all player 1, 20 = show all
    var entryNum = this.pickAnimateForm.value.currentDisplayId;
    this._dataService.revealRound2Value(entryNum);
  }

  showFirstAnswers(form: FormGroup) {
    console.log("Team Num: " + form.get("teamNum")?.value);
    this._dataService.getRound2FirstPlayer(this.yevent, form.get("teamNum")?.value)
                                               .subscribe((data: round2Answers[]) => this.firstPlayerAnswers = data);
    console.log("FPA: " + this.firstPlayerAnswers);
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(Round2countdowndialogComponent, {
      width: '250px',
      data: {countdownValue: this.countdownValue}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The Dialog was closed');
      this.countdownValue = result;
      this.setCountdown(result);
    });
  }

  startCountdown(){
    this._dataService.startCountdown(this.countdownValue);
  }

  stopCountdown(){
    this._dataService.stopCountdown();
  }

  setCountdown(seconds: number){
    this._dataService.setCountdown(Number(seconds));
  }

  changeScreen(name: string){
    this._dataService.changeRound2Page(name);
    if(name == 'prize1')
    {
      this.consolation.play();
    }
  }

}
