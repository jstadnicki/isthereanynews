import { Injectable } from '@angular/core';
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {SubscribedReaderViewModel} from "../../../server/Itan/Api/Controllers/SubscribedReaderViewModel";
import {Observable, Subscription} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ReaderSubscriptionServiceService {

  constructor(
    private msalWrapperService: MsalWrapperService,
    private http: HttpClient,
  ) { }

  async loadReadersAsync(onSuccess, onError) {
    const options = await this.msalWrapperService.getOptionsHeadersAsync();
    this
      .http
      .get<SubscribedReaderViewModel[]>(`${environment.apiUrl}/api/followers`, options)
      .subscribe(r=>onSuccess(r), error => onError(error) );

  }
}
