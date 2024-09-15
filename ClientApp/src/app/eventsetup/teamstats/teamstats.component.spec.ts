import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamstatsComponent } from './teamstats.component';

describe('TeamstatsComponent', () => {
  let component: TeamstatsComponent;
  let fixture: ComponentFixture<TeamstatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeamstatsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamstatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
