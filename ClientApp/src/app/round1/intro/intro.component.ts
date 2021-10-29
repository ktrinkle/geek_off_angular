import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { introDto } from '../../data/data';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { DataService } from 'src/app/data.service';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Store } from '@ngrx/store';
import { selectCurrentEvent } from 'src/app/store';

@Component({
  selector: 'app-intro',
  templateUrl: './intro.component.html',
  styleUrls: ['./intro.component.scss'],
  animations: [
    trigger('fadeInOut', [
      state('void', style({
        opacity: 0
      })),
      transition('void <=> *', animate(1000)),
    ]),
  ]
})

export class Round1IntroComponent implements OnInit, OnDestroy {

  @ViewChild('introVideo', { static: true }) introVideo: ElementRef | undefined;

  currentScreen: string = "";
  currentItem: number = 0;
  seatBelt: boolean = false;
  public yEvent: string = '';
  public teamMasterList: introDto[] = [];
  destroy$: Subject<boolean> = new Subject<boolean>();

  // introVid = new HTMLVideoElement();
  constructor(private route: ActivatedRoute,
    private router: Router,
    private matIconRegistry: MatIconRegistry,
    private domSanitzer: DomSanitizer,
    private dataService: DataService,
    private store: Store) {
    //this.store.dispatch(round1AllTeams({ yEvent: 'e21' }));

    this.matIconRegistry
      .addSvgIcon('geekplane', this.domSanitzer.bypassSecurityTrustResourceUrl('/assets/icon/icon_plane_aa.svg'))
      .addSvgIcon('geekphone', this.domSanitzer.bypassSecurityTrustResourceUrl('/assets/icon/icon_phone_aa.svg'))
      .addSvgIcon('geekmask', this.domSanitzer.bypassSecurityTrustResourceUrl('/assets/icon/icon_facemask_blue.svg'))
      .addSvgIcon('geekasterisk', this.domSanitzer.bypassSecurityTrustResourceUrl('/assets/icon/icon_gtasterisk.svg'));
  }

  ngOnInit(): void {
    //this.store.select(selectRound1Teams).pipe(takeUntil(this.destroy$)).subscribe(x =>
    //  this.teamMasterList = x
    //);

    this.route.params.subscribe(params => {
      this.currentScreen = params['page'];
      console.log('Screen: ' + params['page']);
    });

    // grab list of teams
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.dataService.getRound1IntroTeamList(this.yEvent).pipe(takeUntil(this.destroy$)).subscribe(t =>
          this.teamMasterList = t
        );
      }
    });

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

    connection.on("round1intro", (data: any) => {
      this.changePage(data);
    });

    connection.on("introSeatbelt", (data: any) => {
      this.changeSeatbelt;
    })

    connection.on("round1question", (data: any) => {
      this.goToQuestions(data);
    });

    connection.on("round1Animate", (data: any) => {
      this.changeText();
    });

  }

  changePage(page: any): void {
    this.currentItem = 0;
    this.currentScreen = page;
  }

  playVid() {
    console.log('Playing video');
    this.introVideo?.nativeElement.play();
  }

  goToQuestions(question: number): void {
    console.log('Going to round 1 questions');
    this.router.navigate(['/round1/question/1']);
  }

  changeText() {
    this.currentItem++;
  }

  changeSeatbelt() {
    this.seatBelt == false ? true : false;
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

}
