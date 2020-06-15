import { TestBed } from '@angular/core/testing';

import { AddNewChannelRepositoryService } from './add-new-channel-repository.service';

describe('AddNewChannelRepositoryService', () => {
  let service: AddNewChannelRepositoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddNewChannelRepositoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
