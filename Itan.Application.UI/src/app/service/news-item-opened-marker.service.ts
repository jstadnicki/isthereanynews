import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MsalWrapperService} from "./msal-wrapper.service";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class NewsItemOpenedMarkerService {

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) { }

  async MarkOpen(channelId: string, newsId: string) {
    const options = await this.msalWrapperService.getOptionsHeaders();

    const body = {
      channelId: channelId,
      newsId: newsId,
    };

    this.http
      .post(`${environment.apiUrl}/api/ChannelOpenNews`, body, options)
      .subscribe((r) => {
        console.log(r);
      });
  }

  async MarkOpenWithClick(channelId: string, newsId: string) {
    const options = await this.msalWrapperService.getOptionsHeaders();

    const body = {
      channelId: channelId,
      newsId: newsId,
    };

    this.http
      .post(`${environment.apiUrl}/api/ChannelReadNews/click`, body, options)
      .subscribe((r) => {
        console.log(r);
      });
  }
}
