import { TestBed } from '@angular/core/testing';

import { NewsItemOpenedMarkerService } from './news-item-opened-marker.service';

describe('NewsItemOpenedMarkerService', () => {
  let service: NewsItemOpenedMarkerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewsItemOpenedMarkerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
