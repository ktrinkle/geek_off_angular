import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { CountdownConfig  } from 'ngx-countdown';
import { single } from 'rxjs/operators';
import { Round2controlComponent } from 'src/app/control/round2control/round2control.component';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-round2countdown',
  templateUrl: './round2countdown.component.html',
  styleUrls: ['./round2countdown.component.scss']
})
export class Round2countdownComponent implements OnInit {

  // Need to make this dynamic.
  timeData: number = 0;
  config: CountdownConfig = {};
  showStop: any;

  constructor(private round2control: Round2controlComponent) {
    this.timeData = this.round2control.countdownValue;
   }

  ngOnInit(): void {
    this.config = {
      leftTime: this.timeData,
      demand: true,
      // We need the below to show just the seconds.
      formatDate: ({ date }) => `${date / 1000}`
    };

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events')
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("startCountdown", (data: any) => {
      this.startCountdown();
    });

    connection.on("stopCountdown", (data: any) => {
      this.startCountdown();
    });

  }

  startCountdown(){
    this.config = {
      leftTime: this.timeData,
      demand: false,
      // We need the below to show just the seconds.
      formatDate: ({ date }) => `${date / 1000}`
    };
    this.showStop = true;
  }

  stopCountdown(){
    this.config = {
      leftTime: this.timeData,
      demand: true,
      // We need the below to show just the seconds.
      formatDate: ({ date }) => `${date / 1000}`
    };
    this.showStop = false;
  }

  handleEvent(event: any){
    console.log(event);
  }

}
