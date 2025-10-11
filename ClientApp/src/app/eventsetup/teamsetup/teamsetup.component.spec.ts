import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsetupComponent } from './teamsetup.component';

describe('TeamsetupComponent', () => {
  let component: TeamsetupComponent;
  let fixture: ComponentFixture<TeamsetupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeamsetupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamsetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
