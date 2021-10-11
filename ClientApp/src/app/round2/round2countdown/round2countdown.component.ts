import { Component, OnInit } from '@angular/core';
import { CountdownConfig  } from 'ngx-countdown';

@Component({
  selector: 'app-round2countdown',
  templateUrl: './round2countdown.component.html',
  styleUrls: ['./round2countdown.component.scss']
})
export class Round2countdownComponent implements OnInit {

  timeData = 25;
  config: CountdownConfig = {};
  showStop: any;

  constructor() { }

  ngOnInit(): void {
    this.config = {
      leftTime: this.timeData,
      demand: true,
      // We need the below to show just the seconds.
      formatDate: ({ date }) => `${date / 1000}`
    };
  }

  start(){
    this.config = {
      leftTime: this.timeData,
      demand: false,
      // We need the below to show just the seconds.
      formatDate: ({ date }) => `${date / 1000}`
    };
    this.showStop = true;
  }

  stop(){
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
