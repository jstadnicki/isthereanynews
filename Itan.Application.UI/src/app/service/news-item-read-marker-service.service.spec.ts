import { TestBed } from '@angular/core/testing';

import { NewsItemReadMarkerServiceService } from './news-item-read-marker-service.service';

describe('NewsItemReadMarkerServiceService', () => {
  let service: NewsItemReadMarkerServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewsItemReadMarkerServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
