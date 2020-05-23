import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OwnerPageComponent } from './owner-page.component';

describe('OwnerPageComponent', () => {
  let component: OwnerPageComponent;
  let fixture: ComponentFixture<OwnerPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OwnerPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OwnerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
