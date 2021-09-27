import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round1ContestantComponent } from './contestant.component';

describe('ContestantComponent', () => {
  let component: Round1ContestantComponent;
  let fixture: ComponentFixture<Round1ContestantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round1ContestantComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round1ContestantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
