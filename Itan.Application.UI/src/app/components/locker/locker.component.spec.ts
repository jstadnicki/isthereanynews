import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LockerComponent } from './locker.component';

describe('LockerComponent', () => {
  let component: LockerComponent;
  let fixture: ComponentFixture<LockerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LockerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LockerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
