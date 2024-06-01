import { Component, OnInit } from '@angular/core';
import { QRCodeModule } from 'angularx-qrcode';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'typescript-guid';
import { SafeUrl } from '@angular/platform-browser';


@Component({
  selector: 'app-teamlink',
  templateUrl: './teamlink.component.html',
  styleUrls: ['./teamlink.component.scss']
})
export class TeamlinkComponent implements OnInit {

  public yEvent: string = "";
  public teamGuid: Guid | undefined;
  public qrUrl: SafeUrl = "";
  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      let guidString = params.get('teamGuid');
      this.teamGuid = Guid.parse(guidString ?? "");

      this.yEvent = params.get('yEvent');
    });

  }

  onChangeURL(url: SafeUrl) {
    this.qrUrl = url;
  }


}
