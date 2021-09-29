import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round1ScoreboardComponent } from './scoreboard.component';

describe('Round1ScoreboardComponent', () => {
  let component: Round1ScoreboardComponent;
  let fixture: ComponentFixture<Round1ScoreboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round1ScoreboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round1ScoreboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
