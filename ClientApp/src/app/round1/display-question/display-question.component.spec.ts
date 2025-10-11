import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round1DisplayQuestionComponent } from './display-question.component';

describe('Round1DisplayQuestionComponent', () => {
  let component: Round1DisplayQuestionComponent;
  let fixture: ComponentFixture<Round1DisplayQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round1DisplayQuestionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round1DisplayQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
