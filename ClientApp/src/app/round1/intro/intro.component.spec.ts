import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round1IntroComponent } from './intro.component';

describe('Round1IntroComponent', () => {
  let component: Round1IntroComponent;
  let fixture: ComponentFixture<Round1IntroComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round1IntroComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round1IntroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
