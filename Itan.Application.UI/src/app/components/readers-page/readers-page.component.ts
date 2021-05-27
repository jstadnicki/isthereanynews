import {Component, OnInit} from '@angular/core';
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {ReaderViewModel} from "../../../server/Itan/Core/GetAllReaders/ReaderViewModel";
import {ReaderDetailsViewModel} from "../../../server/Itan/Core/GetReader/ReaderDetailsViewModel";
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {catchError, tap} from "rxjs/operators";
import {of} from "rxjs";

@Component({
  selector: 'app-readers-page',
  templateUrl: './readers-page.component.html',
  styleUrls: ['./readers-page.component.scss']
})
export class ReadersPageComponent implements OnInit {
  isLoggedIn: boolean;
  readers: ReaderViewModel[];
  selectedReader: ReaderViewModel;
  selectedChannel: any;
  readersLoaded: boolean = false;
  selectedReaderDetails: ReaderDetailsViewModel;
  notificationText: string = "";
  notificationSuccessful: boolean = false;
  notificationTimeout: any;

  constructor(
    private msalWrapperService: MsalWrapperService,
    private http: HttpClient,
  ) {
  }

  async ngOnInit() {
    this.msalWrapperService.isLoggedIn.subscribe(v => this.isLoggedIn = v);

    let options = this.msalWrapperService.getOptionsHeaders();
    let url = `${environment.apiUrl}/api/readers`;
    this
      .http
      .get<ReaderViewModel[]>(url, options)
      .pipe(
        tap((r)=>{
          this.readersLoaded = true;
          this.readers = r;
        })
      )
      .subscribe()
  }

  async onReaderClick(reader: ReaderViewModel) {
    this.selectedReader = reader;
    let options = this.msalWrapperService.getOptionsHeaders();
    let url = `${environment.apiUrl}/api/readers/${this.selectedReader.id}`;
    this
      .http
      .get<ReaderDetailsViewModel>(url, options)
      .pipe(
        tap((r)=>{this.selectedReaderDetails = r})
      )
      .subscribe();
  }

  async onChannelSubscribeClick(id: any) {
    const options = this.msalWrapperService.getOptionsHeaders();

    const userId = this.msalWrapperService.getAccountId();
    const body = {
      channelId: id
    };

    this.http
      .post(`${environment.apiUrl}/api/users/${userId}/channels`, body, options)
      .pipe(
        tap(() => this.showNotification(true, "subscription command executed successfully")),
        catchError(x => of(this.showNotification(false, "subscription command executed with error")))
      )
      .subscribe();
  }

  async onSubscribeReader(id: string) {
    const options = this.msalWrapperService.getOptionsHeaders();
    let body = {
      readerId:id
    };
    this
      .http
      .post(`${environment.apiUrl}/api/followers/`, body, options)
      .pipe(
        tap(() => this.showNotification(true, "subscription command executed successfully")),
        catchError(x => of(this.showNotification(false, "subscription command executed with error")))
      )
      .subscribe();
  }

  async onUnsubscribeReader(id: string) {
    const options = this.msalWrapperService.getOptionsHeaders();
    await this
      .http
      .delete(`${environment.apiUrl}/api/followers/${id}`, options)
      .pipe(
        tap(() => this.showNotification(true, "unsubscription command executed successfully")),
        catchError(x => of(this.showNotification(false, "unsubscription command executed with error")))
      )
      .subscribe();
  }

  private showNotification(wasSuccessful: boolean, notificationText: string) {
    this.notificationText = notificationText;
    this.notificationSuccessful = wasSuccessful;
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
