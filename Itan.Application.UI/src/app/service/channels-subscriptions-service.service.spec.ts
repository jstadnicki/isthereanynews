import { TestBed } from '@angular/core/testing';

import { ChannelsSubscriptionsServiceService } from './channels-subscriptions-service.service';

describe('ChannelsSubscriptionsServiceService', () => {
  let service: ChannelsSubscriptionsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChannelsSubscriptionsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
