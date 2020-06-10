import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewChannelComponent } from './add-new-channel.component';

describe('AddNewChannelComponent', () => {
  let component: AddNewChannelComponent;
  let fixture: ComponentFixture<AddNewChannelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddNewChannelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddNewChannelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
