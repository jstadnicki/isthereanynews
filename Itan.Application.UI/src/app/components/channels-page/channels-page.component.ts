import {Component, OnInit} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BroadcastService, MsalService} from "@azure/msal-angular";
import {StripHtmlPipe} from "./strip-html.pipe"

@Component({
  selector: "app-channels-page",
  templateUrl: "./channels-page.component.html",
  styleUrls: ["./channels-page.component.less"],
})
export class ChannelsPageComponent implements OnInit {
  token: string;
  sessionId: string;

  constructor(
    private broadcastService: BroadcastService,
    private http: HttpClient,
    private authService: MsalService
  ) {
  }

  channels: Channel[];
  selectedChannel: Channel;
  news: News[];
  areChannelsLoaded: boolean;
  areNewsLoading: boolean;

  ngOnInit(): void {
    this.areChannelsLoaded = false;
    this.areNewsLoading = false;

    this.broadcastService.subscribe(
      "msal:acquireTokenSuccess",
      (payload) => {
        this.token = payload.accessToken;
        this.sessionId = this.createUUID();
      }
    );

    this.broadcastService.subscribe(
      "msal:acquireTokenFailure",
      (payload) => {
        console.log(payload);
      }
    );

    this.broadcastService.subscribe("msal:loginFailure", (payload) => {
      console.log("ChannelsPageComponent:msal:loginFailure");
      console.log(payload);
    });

    this.broadcastService.subscribe("msal:loginSuccess", (payload) => {
    });

    this.loadChannels();
  }

  async subscribe(channel:Channel){}
  async unsubscribe(channel:Channel){}

  async onChannelClick(channel: Channel) {
    if (channel == this.selectedChannel) {
      return;
    }
    this.selectedChannel = channel;
    this.areNewsLoading = true;
    this.news = null;
    const a = this.authService.getAccount();

    // const accessTokenRequest = {
    //   scopes: [
    //     "https://isthereanynewscodeblast.onmicrosoft.com/api/application_reader",
    //     "https://isthereanynewscodeblast.onmicrosoft.com/api/application_writer",
    //   ],
    //   clientId: "01805485-e711-4975-bbed-d10eb448d097",
    //   authority:
    //     "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup",
    //   redirectUri: "http://localhost:4200",
    //   account: a,
    //   sid: this.sessionId
    // };
    //
    // const x = await this.authService.acquireTokenSilent(accessTokenRequest);
    //
    // const o = this.getOptions(x.accessToken);
    this.http
      .get<News[]>(`https://localhost:5001/api/news/${channel.id}`)
      .subscribe((r) => {
        this.news = r;
        this.areNewsLoading = false;
      });
  }

  private createUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  async onNewsClick(newsItem: News) {
    if (newsItem.content != null) {
      newsItem.contentVisible = !newsItem.contentVisible;
      return;
    }
    newsItem.loading = true;
    const url = newsItem.contentUrl;
    let headers = new HttpHeaders();
    headers.append("Origin", "http://localhost:4200");
    let options = {headers: headers}
    this.http
      .get<NewsContent>(url, options)
      .subscribe(response => {
        newsItem.loading = false;
        newsItem.content = response;
        newsItem.contentVisible = !newsItem.contentVisible;
      });
  }

  display(news: NewsContent): string {
    return news.Description ?? news.Content;
  }

  async loadChannels() {
    const a = this.authService.getAccount();

    // const accessTokenRequest = {
    //   scopes: [
    //     "https://isthereanynewscodeblast.onmicrosoft.com/api/application_reader",
    //     "https://isthereanynewscodeblast.onmicrosoft.com/api/application_writer",
    //   ],
    //   clientId: "01805485-e711-4975-bbed-d10eb448d097",
    //   authority:
    //     "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup",
    //   redirectUri: "http://localhost:4200",
    //   account: a,
    //   sid: this.sessionId
    // };
    // const x = await this.authService.acquireTokenSilent(accessTokenRequest);
    //
    // const o = this.getOptions(x.accessToken);
    this.http
      .get<Channel[]>("https://localhost:5001/api/channels")
      .subscribe((r) => {
        this.channels = r;
        this.areChannelsLoaded = true;
      });
  }

  private getOptions(token: string) {
    return {
      headers: new HttpHeaders({Authorization: `Bearer ${token}`}),
    };
  }
}

class Channel {
  url: string;
  title: string;
  id: string;
  newsCount: number;
}

class News {
  title: string;
  id: string;
  contentUrl: string;
  content: NewsContent;
  loading: boolean = false;
  contentVisible: boolean = false;
  published: Date
}

class NewsContent {
  Content: string;
  Description: string;
  Author: string;
  Link: string;
}
