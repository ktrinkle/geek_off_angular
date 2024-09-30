import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { interval, Subject } from 'rxjs';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { take, takeUntil, map } from 'rxjs/operators';
import { roundCategory, roundThreeCategoryPoints } from 'src/app/data/data';
import { environment } from 'src/environments/environment';
import { Store } from '@ngrx/store';
import { selectCurrentEvent, selectRound3Categories, selectRound3CategoryPoints } from 'src/app/store';
import { round3Categories, round3CategoryPoints } from 'src/app/store/round3/round3.actions';
import * as signalR from '@microsoft/signalr';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-jepintro',
  standalone: true,
  imports: [CommonModule],
  animations: [
    trigger('fadeIn', [
      state('void', style({
        opacity: 0
      })),
      transition('void => *', animate(1000)),
    ]),
    trigger('slideLeft', [
      transition(':enter', [style({ width: 0 }), animate(300)]),
      transition(':leave', [animate(300, style({ width: 0 }))]),
    ]),
  ],
  templateUrl: './jepintro.component.html',
  styleUrl: './jepintro.component.scss'
})
export class JepIntroComponent implements OnInit, OnDestroy{
  destroy$: Subject<boolean> = new Subject<boolean>();

  // dependencies
  store = inject(Store);
  router = inject(Router);

  // control and state management
  yEvent = '';
  categoryScoreMaster: roundThreeCategoryPoints[] = [];
  bigDisplay: roundThreeCategoryPoints[] = [];
  categoryList: roundCategory[] = [];

  // flags
  introVideoVisible = true;

  boardFlyIn = false;

  categoryNumber = 0;
  categoryShow = false;

  // external URIs
  flyInSound = new Audio('https://geekoff2021static.blob.core.windows.net/snd/flyin.mp3');

  ngOnInit(): void {
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.store.dispatch(round3CategoryPoints({ yEvent: this.yEvent }));
        this.store.dispatch(round3Categories({ yEvent: this.yEvent }));
      }
    });

    this.store.select(selectRound3CategoryPoints).pipe(takeUntil(this.destroy$)).subscribe(categoryScoreMaster => {
      this.categoryScoreMaster = categoryScoreMaster;
    });

    this.store.select(selectRound3Categories).pipe(takeUntil(this.destroy$)).subscribe(categories => {
      this.categoryList = categories;
    });

    this.flyInSound.load();

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

    connection.on("round3Animate", () => {
      this.introVideoVisible = false;
      console.log('boardFlyinAnimate called');
      this.boardFlyInAnimate();
    });

    connection.on("round3CategoryChange", (data) => {
      this.categoryNumber = data ?? 0;
      // id is 1-5
    });

    connection.on("round3BigBoard", () => {
      this.boardFlyIn = false;
      this.router.navigate(['/round3jep/board']);
    });

  }

  boardFlyInAnimate(): void {
    // debugger;
    this.bigDisplay = this.categoryScoreMaster.map(
			( cats ) => {
        return ({
          questionNum: cats.questionNum,
          enabled: false,
          ptsposs: cats.ptsposs,
          yEvent: cats.yEvent
        })
      });
    const questionArray = this.shuffleQuestions(this.bigDisplay);
    console.log('questionArray', questionArray);
    // this.flyInSound.play();
    this.boardFlyIn = true;
    const numbers = interval(180);
    const squareCount = numbers.pipe(take(this.bigDisplay.length));
    squareCount.subscribe(x => {
      const questionNum = questionArray[x].questionNum;
      const questionIndex = this.bigDisplay.findIndex(b => b.questionNum === questionNum);
      if (questionIndex !== -1) {
        console.log(questionIndex, new Date);
        this.bigDisplay[questionIndex].enabled = true;
      }
      console.log('questionNum', questionArray[x].questionNum, new Date);
    });
  }

  // fisher-yates algorithm
  shuffleQuestions(array: roundThreeCategoryPoints[]): roundThreeCategoryPoints[] {
    console.log('shuffleQuestions started');
    const workingArray: roundThreeCategoryPoints[] = structuredClone(array);
    for (let i = array.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [workingArray[i], workingArray[j]] = [workingArray[j], workingArray[i]];
    }
    console.log('workingArray', workingArray);
    return workingArray;
  }

  animateCategoryCounter(): void {
    // not implemented yet
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
