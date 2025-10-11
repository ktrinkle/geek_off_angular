import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../app/data.service';
import { round23Scores } from '../../../app/data/data';
import { selectCurrentEvent } from '../../../app/store';
import { Store } from '@ngrx/store';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-teamstats',
  templateUrl: './teamstats.component.html',
  styleUrls: ['./teamstats.component.scss']
})
export class TeamstatsComponent implements OnInit {

  public stats: round23Scores[] = [];
  private yEvent = '';
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private _dataService: DataService, private store: Store ) {}

  ngOnInit(): void {
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this._dataService.getTeamStats(this.yEvent).subscribe(t => {
          this.stats = t;
        }
      )}
    });
  }
}
