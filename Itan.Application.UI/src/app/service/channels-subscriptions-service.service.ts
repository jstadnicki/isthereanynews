import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MsalWrapperService} from "./msal-wrapper.service";

@Injectable({
  providedIn: 'root'
})
export class ChannelsSubscriptionsServiceService {

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) { }

  public async subscribeToChannel(channelId: string) {
    const options = await this.msalWrapperService.getOptionsWriteHeaders();

    const userId = this.msalWrapperService.getAccountId();
    const body = {
      channelId: channelId
    };

    this.http
      .post(`https://itan-app-service-webapi.azurewebsites.net/api/users/${userId}/channels`, body, options)
      .subscribe((r) => {
        console.log(r);
      });
  }

  async unsubscribeFromChannel(channelId: string) {
    const options = await this.msalWrapperService.getOptionsWriteHeaders();

    const userId = this.msalWrapperService.getAccountId();

    this.http
      .delete(`https://itan-app-service-webapi.azurewebsites.net/api/users/${userId}/channels/${channelId}`, options)
      .subscribe((r) => {
        console.log(r);
      });
  }
}
