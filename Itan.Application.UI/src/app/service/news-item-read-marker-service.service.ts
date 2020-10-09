import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MsalWrapperService} from "./msal-wrapper.service";

@Injectable({
  providedIn: 'root'
})
export class NewsItemReadMarkerServiceService {

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) { }

  async MarkRead(channelId: string, newsId: string) {
    const options = await this.msalWrapperService.getOptionsWriteHeaders();

    const body = {
      channelId: channelId,
      newsId: newsId,
    };

    this.http
      .post(`https://localhost:5001/api/ChannelReadNews`, body, options)
      .subscribe((r) => {
        console.log(r);
      });
  }
}
