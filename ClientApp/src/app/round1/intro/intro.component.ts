import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { Store } from '@ngrx/store';
import { round1AllTeams, round1AllTeamsSuccess } from '../../store/round1/round1.actions';
import { introDto } from '../../data/data';
import { selectRound1Teams } from 'src/app/store';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MatIconRegistry } from '@angular/material/icon';
import { DataService } from 'src/app/data.service';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-intro',
  templateUrl: './intro.component.html',
  styleUrls: ['./intro.component.scss'],
  animations: []
})
export class Round1IntroComponent implements OnInit, OnDestroy {

  currentScreen: string = "";
  public yevent: string = sessionStorage.getItem('event') ?? '';
  public teamMasterList: introDto[] = [];
  constructor(private store: Store, private route: ActivatedRoute, private router: Router, private matIconRegistry: MatIconRegistry, private dataService: DataService) {
    //this.store.dispatch(round1AllTeams({ yEvent: 'e21' }));

    this.matIconRegistry
      .addSvgIcon('geekplane','../../assets/img/Technology-White-rgb-14.png')
      .addSvgIcon('geekphone','../../assets/img/Travel-White-rgb-01.png');
   }

   destroy$: Subject<boolean> = new Subject<boolean>();

  ngOnInit(): void {
    //this.store.select(selectRound1Teams).pipe(takeUntil(this.destroy$)).subscribe(x =>
    //  this.teamMasterList = x
    //);

    this.route.params.subscribe(params => {
      this.currentScreen = params['page'];
      console.log('Screen: ' + params['page']);
    });

    // grab list of teams
    this.dataService.getRound1IntroTeamList(this.yevent).pipe(takeUntil(this.destroy$)).subscribe(t =>
      this.teamMasterList = t
    );

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + 'event')
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round1intro", (data: any) => {
      this.changePage(data);
    });

    connection.on("round1question", (data: any) => {
      this.goToQuestions(data);
    });

  }

  changePage(page: any): void {
    this.currentScreen = page;
  }

  goToQuestions(question: number): void {
    this.router.navigate(['/round1/contestant']);
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

}
