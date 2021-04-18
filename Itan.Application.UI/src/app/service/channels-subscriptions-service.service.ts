import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MsalWrapperService} from "./msal-wrapper.service";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ChannelsSubscriptionsServiceService {

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) { }

  public async subscribeToChannel(channelId: string) {
    const options = await this.msalWrapperService.getOptionsHeadersAsync();

    const userId = this.msalWrapperService.getAccountId();
    const body = {
      channelId: channelId
    };

    this.http
      .post(`${environment.apiUrl}/api/users/${userId}/channels`, body, options)
      .subscribe((r) => {
      });
  }

  async unsubscribeFromChannel(channelId: string) {
    const options = await this.msalWrapperService.getOptionsHeadersAsync();

    const userId = this.msalWrapperService.getAccountId();

    this.http
      .delete(`${environment.apiUrl}/api/users/${userId}/channels/${channelId}`, options)
      .subscribe((r) => {
      });
  }
}
