import {Component, OnInit} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BroadcastService, MsalService} from "@azure/msal-angular";
import {StripHtmlPipe} from "./strip-html.pipe"
import {MsalWrapperService} from "../../service/msal-wrapper.service";

@Component({
  selector: "app-channels-page",
  templateUrl: "./channels-page.component.html",
  styleUrls: ["./channels-page.component.less"],
})
export class ChannelsPageComponent implements OnInit {
  // token: string;
  // sessionId: string;

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) {
  }

  channels: Channel[];
  selectedChannel: Channel;
  news: News[];
  areChannelsLoaded: boolean;
  areNewsLoading: boolean;

  async ngOnInit(): Promise<void> {
    this.areChannelsLoaded = false;
    this.areNewsLoading = false;
    await this.loadChannels();
  }

  async subscribe(channel: Channel) {
    const options = await this.msalWrapperService.getOptionsWriteHeaders();

    const userId = this.msalWrapperService.getAccountId();
    const body = {
      channelId: channel.id
    };

    this.http
      .post(`https://localhost:5001/api/users/${userId}/channels`, body, options)
      .subscribe((r) => {
        console.log(r);
      });

  }

  async unsubscribe(channel: Channel) {
  }

  async onChannelClick(channel: Channel) {
    if (channel == this.selectedChannel) {
      return;
    }
    this.selectedChannel = channel;
    this.areNewsLoading = true;
    this.news = null;

    this.http
      .get<News[]>(`https://localhost:5001/api/news/${channel.id}`)
      .subscribe((r) => {
        this.news = r;
        this.areNewsLoading = false;
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
    this.http
      .get<Channel[]>("https://localhost:5001/api/channels")
      .subscribe((r) => {
        this.channels = r;
        this.areChannelsLoaded = true;
      });
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
