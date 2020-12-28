import {Component, OnInit} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {ChannelsSubscriptionsServiceService} from "../../service/channels-subscriptions-service.service";
import { environment } from '../../../environments/environment';

@Component({
  selector: "app-channels-page",
  templateUrl: "./channels-page.component.html",
  styleUrls: ["./channels-page.component.less"],
})
export class ChannelsPageComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService,
    private channelsSubscriptionsServiceService: ChannelsSubscriptionsServiceService
  ) {
  }

  channels: Channel[];
  selectedChannel: Channel;
  news: News[];
  areChannelsLoaded: boolean;
  areNewsLoading: boolean;
  displayAddNewChannel: boolean = false;
  isLoggedIn: boolean = false;

  async ngOnInit(): Promise<void> {
    this.areChannelsLoaded = false;
    this.areNewsLoading = false;
    await this.loadChannels();

    this.msalWrapperService.isLoggedIn.subscribe(v => this.isLoggedIn = v);
  }

  async subscribe(channel: Channel) {
    await this.channelsSubscriptionsServiceService.subscribeToChannel(channel.id);
  }

  async unsubscribe(channel: Channel) {
    await this.channelsSubscriptionsServiceService.unsubscribeFromChannel(channel.id);
  }

  showAddNewChannel() {
    if(!this.isLoggedIn)
      return false;
    this.selectedChannel = null;
    this.news = null;
    this.displayAddNewChannel = true;
  }

  async onChannelClick(channel: Channel) {
    this.displayAddNewChannel = false;
    if (channel == this.selectedChannel) {
      return;
    }
    this.selectedChannel = channel;
    this.areNewsLoading = true;
    this.news = null;

    this.http
      .get<News[]>(`${environment.apiUrl}/api/news/${channel.id}`)
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
    let options = {headers: headers}
    this.http
      .get<NewsContent>(url, options)
      .subscribe(response => {
        newsItem.loading = false;
        newsItem.content = response;
        newsItem.contentVisible = !newsItem.contentVisible;

        var tempDiv = document.createElement('div');
        tempDiv.innerHTML = this.display(response);
        var firstImage = tempDiv.getElementsByTagName('img')[0]
        var imgSrc = firstImage ? firstImage.src : "";

        newsItem.content.Image = imgSrc;
      });
  }

  hasImage(news: NewsContent): boolean {
    return news.Image && news.Image.length > 0;
  }

  getImageSrc(news: NewsContent): string {
    return news.Image;
  }

  onExternalLinkClick(newsItan: News) {
    window.open(newsItan.link);
  }

  display(news: NewsContent): string {
    return news.Description ?? news.Content;
  }

  async loadChannels() {
    this.http
      .get<Channel[]>(`${environment.apiUrl}/api/channels`)
      .subscribe((r) => {
        this.channels = r;
        this.areChannelsLoaded = true;
      });
  }

  displayTitleOrDescriptionOrUrl(selectedChannel: Channel) {
    if(selectedChannel.title!=null && selectedChannel.title.length>0)
      return selectedChannel.title;
    if(selectedChannel.description!=null && selectedChannel.description.length>0)
      return selectedChannel.description;
    return selectedChannel.url;
  }
}

class Channel {
  url: string;
  title: string;
  id: string;
  newsCount: number;
  description:string;
}

class News {
  title: string;
  id: string;
  contentUrl: string;
  content: NewsContent;
  loading: boolean = false;
  contentVisible: boolean = false;
  published: Date
  link: string;
}

class NewsContent {
  Content: string;
  Description: string;
  Image: string;
  Author: string;
  Link: string;
}
