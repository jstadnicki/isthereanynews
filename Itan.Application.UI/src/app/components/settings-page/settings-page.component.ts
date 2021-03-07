import {Component, OnInit} from '@angular/core';
import {SettingsServiceService} from "./settings-service.service";
import {ReaderSettings} from "./reader.settings";
import {UpdatedNews} from "./updated.news";
import {SquashUpdate} from "./squash.update";

@Component({
  selector: 'app-settings-page',
  templateUrl: './settings-page.component.html',
  styleUrls: ['./settings-page.component.less']
})
export class SettingsPageComponent implements OnInit {
  readerSettings: ReaderSettings;
  settingsUpdatedNews = UpdatedNews;
  settingsSquashUpdate = SquashUpdate;
  notificationText: string="";
  notificationSuccessful: boolean;
  notificationTimeout: any;

  constructor(private settingService: SettingsServiceService) {
  }

  async ngOnInit(): Promise<void> {
    (await this.settingService.getReaderSettings())
      .subscribe(r => {
        this.readerSettings = r;
      });
  }

  async onShowUpdatedNews() {
    (await this.settingService.updateShowsUpdated(UpdatedNews.Show))
      .subscribe(r=>{
        this.showNotificationToast();
      })
  }

  async onIgnoreUpdatedNews() {
    (await this.settingService.updateShowsUpdated(UpdatedNews.Ignore))
      .subscribe(r => {
        this.showNotificationToast();
      })
  }

  async onSquashNews() {
    (await this.settingService.squashNewsUpdates(SquashUpdate.Squash))
      .subscribe(r => {
        this.showNotificationToast();
      })
  }

  async onShowAllNews() {
    (await this.settingService.squashNewsUpdates(SquashUpdate.Show))
      .subscribe(r => {
        this.showNotificationToast();
      })
  }

  getShowUpdatedNews(expected:UpdatedNews): boolean {
    return this.readerSettings.showUpdatedNews == expected;
  }

  getSquashUpdatedNews(expected:SquashUpdate): boolean {
    return this.readerSettings.squashNewsUpdates == expected;
  }

  private showNotificationToast() {
    this.notificationText='Saved successfully';
    this.notificationSuccessful=true;
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

}

