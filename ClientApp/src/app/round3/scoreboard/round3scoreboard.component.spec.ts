import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round3scoreboardComponent } from './round3scoreboard.component';

describe('ScoreboardComponent', () => {
  let component: Round3scoreboardComponent;
  let fixture: ComponentFixture<Round3scoreboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round3scoreboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round3scoreboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
