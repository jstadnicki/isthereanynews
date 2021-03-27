import {Component, OnInit} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {ChannelsSubscriptionsServiceService} from "../../service/channels-subscriptions-service.service";
import {environment} from '../../../environments/environment';
import {ChannelViewModel} from '../../../server/Itan/Core/ChannelViewModel';
import {NewsViewModel} from '../../../server/Itan/Core/NewsViewModel';

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

  channels: ChannelViewModel[];
  selectedChannel: ChannelViewModel;
  news: News[];
  areChannelsLoaded: boolean;
  areNewsLoading: boolean;
  displayAddNewChannel: boolean = false;
  isLoggedIn: boolean = false;
  notificationText: string = "";
  notificationSuccessful: boolean = false;
  notificationTimeout: any;

  async ngOnInit(): Promise<void> {
    this.areChannelsLoaded = false;
    this.areNewsLoading = false;
    await this.loadChannels();

    this.msalWrapperService.isLoggedIn.subscribe(v => this.isLoggedIn = v);
  }

  async subscribe(channel: ChannelViewModel) {
    this.channelsSubscriptionsServiceService.subscribeToChannel(channel.id)
      .then(() => this.showSubscribeNotification(true))
      .catch(() => this.showSubscribeNotification(false));
  }

  async unsubscribe(channel: ChannelViewModel) {
    this.channelsSubscriptionsServiceService.unsubscribeFromChannel(channel.id)
      .then(() => this.showUnsubscribeNotification(true))
      .catch(() => this.showUnsubscribeNotification(false));
  }

  showAddNewChannel() {
    if (!this.isLoggedIn)
      return false;
    this.selectedChannel = null;
    this.news = null;
    this.displayAddNewChannel = true;
  }

  async onChannelClick(channel: ChannelViewModel) {
    this.displayAddNewChannel = false;
    this.closeNotification();
    if (channel == this.selectedChannel) {
      return;
    }
    this.selectedChannel = channel;
    this.areNewsLoading = true;
    this.news = null;

    this.http
      .get<NewsViewModel[]>(`${environment.apiUrl}/api/news/${channel.id}`)
      .subscribe((r) => {
        this.news = r.map(vm => new News(vm))
        this.areNewsLoading = false;
      });
  }

  async onNewsClick(newsItem: News) {
    if (newsItem.content != null) {
      newsItem.contentVisible = !newsItem.contentVisible;
      return;
    }
    newsItem.loading = true;
    const url = newsItem.viewModel.contentUrl;
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
    window.open(newsItan.viewModel.link);
  }

  display(news: NewsContent): string {
    return news.Description ?? news.Content;
  }

  async loadChannels() {
    this.http
      .get<ChannelViewModel[]>(`${environment.apiUrl}/api/channels`)
      .subscribe((r) => {
        this.channels = r;
        this.areChannelsLoaded = true;
      });
  }

  displayTitleOrDescriptionOrUrl(selectedChannel: ChannelViewModel) {
    if (selectedChannel.title != null && selectedChannel.title.length > 0)
      return selectedChannel.title;
    if (selectedChannel.description != null && selectedChannel.description.length > 0)
      return selectedChannel.description;
    return selectedChannel.url;
  }

  getNewsTitle(newsItem: News) {
    if (newsItem.viewModel.title != null && newsItem.viewModel.title.length > 0) {
      return newsItem.viewModel.title;
    }
    return newsItem.viewModel.link;
  }

  private showUnsubscribeNotification(successful: boolean) {
    if (successful) {
      this.notificationText = "unsubscription command executed successfully";
    } else {
      this.notificationText = "unsubscription command executed with error";
    }
    this.notificationSuccessful = successful;

    this.setNotificationClearTimer();
  }

  private showSubscribeNotification(successful: boolean) {
    if (successful) {
      this.notificationText = "subscription command executed successfully";
    } else {
      this.notificationText = "subscription command executed with error";
    }
    this.notificationSuccessful = successful;
    this.setNotificationClearTimer();
  }

  private setNotificationClearTimer() {
    if (this.notificationTimeout != null) {
      clearTimeout(this.notificationTimeout);
      this.notificationTimeout = null;
    }
    this.notificationTimeout = setTimeout(() => {
        this.closeNotification();
      }
      , 3500);
  }

  closeNotification() {
    if (this.notificationTimeout != null) {
      clearTimeout(this.notificationTimeout);
      this.notificationTimeout = null;
    }
    this.notificationText = "";
    this.notificationTimeout = null;
  }
}

class News {
  constructor(vm: NewsViewModel) {
    this.viewModel = vm;
  }

  viewModel: NewsViewModel;
  content: NewsContent;
  contentVisible: boolean;
  loading: boolean;
}

class NewsContent {
  Content: string;
  Description: string;
  Image: string;
  Author: string;
  Link: string;
}

