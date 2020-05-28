import { TestBed } from '@angular/core/testing';

import { MsalWrapperService } from './msal-wrapper.service';

describe('MsalWrapperService', () => {
  let service: MsalWrapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsalWrapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
