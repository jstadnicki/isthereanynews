import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ReadersSubscriptionsServiceService {
  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) { }

  async subscribeToReaderAsync(readerId: string){
    const options = this.msalWrapperService.getOptionsHeaders();
    let body={readerId};
    return this.http.post(`${environment.apiUrl}/api/followers/`, body, options).subscribe();
  }

  async unsubscribeToReaderAsync(readerId: string):Promise<any> {
    const options = this.msalWrapperService.getOptionsHeaders();
    return await this.http.delete(`${environment.apiUrl}/api/followers/${readerId}`, options).subscribe();
  }
}
