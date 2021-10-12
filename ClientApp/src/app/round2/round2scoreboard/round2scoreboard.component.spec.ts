import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round2scoreboardComponent } from './round2scoreboard.component';

describe('Round2scoreboardComponent', () => {
  let component: Round2scoreboardComponent;
  let fixture: ComponentFixture<Round2scoreboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round2scoreboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round2scoreboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
