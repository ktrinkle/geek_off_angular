import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';
import { DataService } from 'src/app/data.service';
import { Store } from '@ngrx/store';
import { selectCurrentEvent } from 'src/app/store';

@Component({
  selector: 'app-pregame',
  templateUrl: './pregame.component.html',
  styleUrls: ['./pregame.component.scss']
})
export class PregameComponent implements OnInit, OnDestroy {

  currentPage = '1';
  currentPageName = 'Intro page 1';
  teamList: any[] = [];
  seatBelt = false;
  yEvent = '';
  audio = new Audio();
  seatBeltSound = new Audio();
  listofPages: { key: string, value: string }[] = [
    { key: '1', value: 'Intro page 1' },
    { key: '2', value: 'Intro page 2' },
    { key: 'open', value: 'Seatbelt sign' },
    { key: 'open2', value: 'Intro video' },
    { key: 'teamList', value: 'List of teams' },
    { key: 'rule1', value: 'Rules page 1' },
    { key: 'rule2', value: 'Rules page 2' },
    { key: 'rule3', value: 'Rules page 3' },
  ];

  pageForm: UntypedFormGroup = new UntypedFormGroup({
    pageName: new UntypedFormControl('')
  });

  fundForm: UntypedFormGroup = new UntypedFormGroup({
    teamNumber: new UntypedFormControl(''),
    dollarAmount: new UntypedFormControl('', Validators.pattern('[0-9].'))
  });

  fundFormStatus = '';

  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private router: Router, private dataService: DataService, private store: Store) { }

  ngOnInit(): void {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.dataService.getRound1IntroTeamList(this.yEvent).pipe(takeUntil(this.destroy$)).subscribe(t =>
          this.teamList = t
        );
      }
    });

    this.audio.src = 'https://geekoff2021static.blob.core.windows.net/snd/top_of_hour.mp3';
    this.seatBeltSound.src = 'https://geekoff2021static.blob.core.windows.net/snd/seatbelt.mp3';
    this.audio.load();
    this.seatBeltSound.load();

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events', { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    // we only want to flip automatically if we have to
    connection.on("round1ShowAnswerChoices", () => {
      console.log('showanswer');
    });

  }

  changeScreen() {
    this.dataService.changeIntroPage(this.pageForm.value.pageName);
    this.currentPage = this.pageForm.value.pageName;
  }

  changeSeatBelt() {
    this.seatBelt == false ? true : false;
    this.dataService.changeSeatbelt();
    this.seatBeltSound.play();
  }

  animateText() {
    this.dataService.changeAnimation();
  }

  playCbsSound() {
    this.audio.play();
  }

  moveToRound1() {
    this.dataService.changeRound1QuestionStatus(this.yEvent, 1, 0).subscribe();
    this.router.navigate(['/control/round1']);
  }

  updateDollarRaised() {
    const teamNum = this.fundForm.value.teamNumber;
    const dollarAmount = this.fundForm.value.dollarAmount;
    this.dataService.updateDollarAmount(this.yEvent, teamNum, dollarAmount).subscribe(data => {
      this.fundFormStatus = data;
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
