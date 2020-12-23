import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {NewsItemReadMarkerServiceService} from "../../service/news-item-read-marker-service.service";
import {environment} from "../../../environments/environment";
import {FormGroup, FormControl, Validators} from '@angular/forms';

@Component({
  selector: 'app-subscriptions-page',
  templateUrl: './subscriptions-page.component.html',
  styleUrls: ['./subscriptions-page.component.scss']
})
export class SubscriptionsPageComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService,
    private newsReadMarker: NewsItemReadMarkerServiceService) {
  }

  channels: Channel[];
  selectedChannel: Channel;
  news: News[];
  areChannelsLoaded: boolean;
  areNewsLoading: boolean;
  importOpml: boolean = false;
  importForm: FormGroup = new FormGroup({
    inputFile: new FormControl(),
    buttonUpload: new FormControl(),
  });

  async ngOnInit(): Promise<void> {
    this.areChannelsLoaded = false;
    this.areNewsLoading = false;
    await this.loadChannels();
  }

  async unsubscribe(channel: Channel) {
  }

  async onChannelClick(channel: Channel) {
    this.importOpml = false;

    if (channel == this.selectedChannel) {
      return;
    }
    this.selectedChannel = channel;
    this.areNewsLoading = true;
    this.news = null;
    var options = await this.msalWrapperService.getOptionsHeaders();
    this.http
      .get<News[]>(`${environment.apiUrl}/api/UnreadNews/${channel.id}`, options)
      .subscribe((r) => {
        this.news = r;
        this.areNewsLoading = false;
      });
  }

  async onImportClick() {
    this.selectedChannel = null;
    this.importOpml = true;
    this.news = null;
  }

  async handleFileInput(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.importForm.patchValue({
        inputFile: file
      });
    }
  }

  async onImportSubmit() {
    const formData = new FormData();
    let fileSourceValue = this.importForm.get('inputFile').value;
    formData.append('file', fileSourceValue);

    var options = await this.msalWrapperService.getOptionsHeaders();
    this.http.post(`${environment.apiUrl}/api/subscriptions/import`, formData, options)
      .subscribe(res => {
        console.log(res);
        alert('Uploaded Successfully.');
      }, err => {
        console.error(err);
      });
  }

  async onNewsClick(newsItem: News) {

    if (newsItem.content != null) {
      newsItem.contentVisible = !newsItem.contentVisible;
      return;
    }

    if (newsItem.read == false) {
      await this.newsReadMarker.MarkRead(this.selectedChannel.id, newsItem.id);
    }
    newsItem.read = true;
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
      });
  }

  display(news: NewsContent): string {
    return news.Description ?? news.Content;
  }

  async loadChannels() {
    const userId = this.msalWrapperService.getAccountId();
    var options = await this.msalWrapperService.getOptionsHeaders();

    this.http
      .get<Channel[]>(`${environment.apiUrl}/api/subscriptions/${userId}`, options)
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
  read: boolean = true;
}


class NewsContent {
  Content: string;
  Description: string;
  Author: string;
  Link: string;
}
