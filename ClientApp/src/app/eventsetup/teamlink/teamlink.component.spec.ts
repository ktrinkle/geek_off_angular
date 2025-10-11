import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamlinkComponent } from './teamlink.component';

describe('TeamlinkComponent', () => {
  let component: TeamlinkComponent;
  let fixture: ComponentFixture<TeamlinkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeamlinkComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamlinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
