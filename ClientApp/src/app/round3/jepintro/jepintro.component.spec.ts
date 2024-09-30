import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JepintroComponent } from './jepintro.component';

describe('JepintroComponent', () => {
  let component: JepintroComponent;
  let fixture: ComponentFixture<JepintroComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JepintroComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(JepintroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
