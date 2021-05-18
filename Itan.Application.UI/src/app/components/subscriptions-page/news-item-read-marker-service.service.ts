import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class NewsItemReadMarkerServiceService {

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) { }

  async MarkRead(channelId: string, newsId: string) {
    const options = this.msalWrapperService.getOptionsHeaders();

    const body = {
      channelId: channelId,
      newsId: newsId,
    };

    this.http
      .post(`${environment.apiUrl}/api/ChannelReadNews`, body, options)
      .subscribe((r) => {
        console.log(r);
      });
  }

  async MarkUnreadAsRead(channelId: string, newsId: string[]) {
    const options = this.msalWrapperService.getOptionsHeaders();

    const body = {
      channelId: channelId,
      newsId: newsId,
    };

    this.http
      .post(`${environment.apiUrl}/api/ChannelReadNews/skipped`, body, options)
      .subscribe((r) => {
        console.log(r);
      });
  }
}
