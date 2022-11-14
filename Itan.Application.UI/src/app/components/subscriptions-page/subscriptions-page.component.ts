import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {environment} from "../../../environments/environment";
import {UntypedFormGroup, UntypedFormControl, Validators} from '@angular/forms';

import {SubscribedChannelViewModel} from '../../../server/Itan/Core/SubscribedChannelViewModel';
import {NewsViewModel} from '../../../server/Itan/Core/NewsViewModel';
import {ReaderSubscriptionServiceService} from "./reader-subscription-service.service";
import {SubscribedReaderViewModel} from "../../../server/Itan/Api/Controllers/SubscribedReaderViewModel";
import {FollowerActivityViewModel} from "../../../server/Itan/Core/GetFollowerActivity/FollowerActivityViewModel";
import {NewsItemReadMarkerServiceService} from "./news-item-read-marker-service.service";
import {NewsItemOpenedMarkerService} from "./news-item-opened-marker.service";
import {catchError, tap} from "rxjs/operators";
import {of} from "rxjs";

@Component({
  selector: 'app-subscriptions-page',
  templateUrl: './subscriptions-page.component.html',
  styleUrls: ['./subscriptions-page.component.scss']
})
export class SubscriptionsPageComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService,
    private newsReadMarker: NewsItemReadMarkerServiceService,
    private newsOpenedMarker: NewsItemOpenedMarkerService,
    private readerSubscriptionServiceService: ReaderSubscriptionServiceService
  ) {
  }

  channels: Channel[];
  readers: SubscribedReaderViewModel[];
  selectedChannel: Channel;
  selectedReader: SubscribedReaderViewModel;
  readerActivities: FollowerActivityViewModel[];
  selectedActivity:FollowerActivityViewModel;
  news: News[];
  areChannelsLoaded: boolean=false;
  areReadersLoaded: boolean=false;
  areNewsLoading: boolean=false;
  areActivitiesLoading:boolean=false;
  notificationText: string = "";
  notificationSuccessful: boolean = false;
  notificationTimeout: any;
  importOpml: boolean = false;
  importForm: UntypedFormGroup = new UntypedFormGroup({
    inputFile: new UntypedFormControl(),
    buttonUpload: new UntypedFormControl(),
  });
  isImporting: boolean = false;

  async ngOnInit(): Promise<void> {
    await this.loadChannels();
    this.readerSubscriptionServiceService.loadReadersAsync((r)=>{
      this.readers=r;
      this.areReadersLoaded=true;
    }, ()=>{});
  }

  async unsubscribe(channel: Channel) {
    // const options = this.msalWrapperService.getOptionsHeaders();

    // const userId = this.msalWrapperService.getAccountId();

    // this.http
    //   .delete(`${environment.apiUrl}/api/users/${userId}/channels/${channel.viewModel.id}`, options)
    //   .pipe(
    //     tap(()=>this.showUnsubscribeNotification(true)),
    //     catchError(()=>of(this.showUnsubscribeNotification(false)))
    //   )
    //   .subscribe();
  }

  async onChannelClick(channel: Channel) {
    this.closeNotification();
    this.importOpml = false;

    if (channel == this.selectedChannel) {
      return;
    }
    this.selectedChannel = channel;
    this.areNewsLoading = true;
    this.news = null;
    this.readerActivities=null;
    var options = this.msalWrapperService.getOptionsHeaders();
    this.http
      .get<NewsViewModel[]>(`${environment.apiUrl}/api/UnreadNews/${channel.viewModel.id}`, options)
      .subscribe((r) => {
        this.news = r.map(vm=>new News(vm))
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
    this.isImporting = true;

    var options = this.msalWrapperService.getOptionsHeaders();
    this.http.post(`${environment.apiUrl}/api/subscriptions/import`, formData, options)
      .subscribe(res => {
        console.log(res);
        alert('Uploaded Successfully.');
        this.isImporting = false;
      }, err => {
        console.error(err);
        this.isImporting = false;
      });
  }

  async onNewsClick(newsItem: News) {
    if (newsItem.content != null) {
      newsItem.contentVisible = !newsItem.contentVisible;
      return;
    }

    if (newsItem.read == false) {
      this.newsReadMarker.MarkRead(this.selectedChannel.viewModel.id, newsItem.viewModel.id)
        .then(()=>{
          this.channels.find(c=>c.viewModel.id == this.selectedChannel.viewModel.id).viewModel.newsCount--;
        })
    }
    newsItem.read = true;
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
      });
  }

  async markUnreadAsRead(channel: Channel) {
    let unread = this.news.filter(f => f.read == false);
    let unreadIds = unread.map(m => m.viewModel.id);
    this.newsReadMarker.MarkUnreadAsRead(channel.viewModel.id, unreadIds)
      .then(() => {
        this.news.forEach(n => n.read = true);
        this.channels.find(c => c.viewModel.id == channel.viewModel.id).viewModel.newsCount = 0;
        this.showMarkAsReadNotification(true);
      })
      .catch(()=>this.showMarkAsReadNotification(false));
  }

  async onActivityExternalLinkClick(activity:FollowerActivityViewModel){
    await this.newsOpenedMarker.MarkOpen(activity.channelId, activity.newsId);
    window.open(activity.link);
  }

  async onExternalLinkClick(news: News) {
    await this.newsOpenedMarker.MarkOpen(this.selectedChannel.viewModel.id, news.viewModel.id);

    if(!news.read){
      this.newsOpenedMarker.MarkOpenWithClick(this.selectedChannel.viewModel.id, news.viewModel.id)
        .then(()=> {
          this.channels.find(c=>c.viewModel.id == this.selectedChannel.viewModel.id).viewModel.newsCount--;
          news.read = true;
        });

    }
    window.open(news.viewModel.link);
  }

  display(news: NewsContent): string {
    return news.Description ?? news.Content;
  }

  async loadChannels() {
    let options = this.msalWrapperService.getOptionsHeaders();
    this.http
      .get<SubscribedChannelViewModel[]>(`${environment.apiUrl}/api/subscriptions`, options)
      .subscribe((r) => {
        this.channels = r.map(vm=>new Channel(vm));
        this.areChannelsLoaded = true;
      });
  }

  displayTitleOrDescriptionOrUrl(selectedChannel: Channel):string {
    if(selectedChannel.viewModel.title!=null && selectedChannel.viewModel.title.length>0)
      return selectedChannel.viewModel.title;
    if(selectedChannel.viewModel.description!=null && selectedChannel.viewModel.description.length>0)
      return selectedChannel.viewModel.description;
    return selectedChannel.viewModel.url;
  }

  getNewsTitle(newsItem: News) {
    var postfix = "";
    if(newsItem.viewModel.originalPostId!=null){
      postfix = " (*** UPDATE ***)"
    }
    if(newsItem.viewModel.title!= null && newsItem.viewModel.title.length>0){
      return newsItem.viewModel.title + postfix;
    }
    return newsItem.viewModel.link + postfix;
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
  private setNotificationClearTimer() {
    if(this.notificationTimeout!=null){
      clearTimeout(this.notificationTimeout);
      this.notificationTimeout = null;
    }

    this.notificationTimeout = setTimeout(() => {
       this.closeNotification();
      }
      , 3500);
  }

  closeNotification() {
    if(this.notificationTimeout!=null) {
      clearTimeout(this.notificationTimeout);
    }
    this.notificationText = "";
    this.notificationTimeout = null;
  }

  private showMarkAsReadNotification(successful: boolean) {
    if (successful) {
      this.notificationText = "marking unread as read command executed successfully";
    } else {
      this.notificationText = "marking unread as read command executed with error";
    }
    this.notificationSuccessful = successful;

    this.setNotificationClearTimer();  }

  onReaderClick(reader: SubscribedReaderViewModel) {
    this.readerSubscriptionServiceService.getReaderActivityAsync(reader.personId,r=>{
      this.readerActivities = r;
      this.selectedChannel=null;
      this.news=null;
    }, e=>{});
  }
}

class Channel{
  constructor(vm:SubscribedChannelViewModel) {
    this.viewModel = vm;
  }
  viewModel:SubscribedChannelViewModel;
}

class News {
  constructor(vm: NewsViewModel) {
    this.viewModel = vm;
  }

  viewModel: NewsViewModel;
  content: NewsContent=null;

  read: boolean=false;
  contentVisible: boolean=false;
  loading: boolean=false;
}

class NewsContent {
  Content: string='';
  Description: string='';
  Author: string='';
  Link: string='';
}
