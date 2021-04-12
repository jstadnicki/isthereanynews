import {Component, OnInit} from '@angular/core';
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {ReadersRepositoryService} from "./readers-repository.service";
import {ReaderViewModel} from "../../../server/Itan/Core/GetAllReaders/ReaderViewModel";
import {ReaderDetailsViewModel} from "../../../server/Itan/Core/GetReader/ReaderDetailsViewModel";
import {ChannelsSubscriptionsServiceService} from "../../service/channels-subscriptions-service.service";
import {ChannelViewModel} from "../../../server/Itan/Core/ChannelViewModel";

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
  notificationText: string="";
  notificationSuccessful: boolean=false;
  notificationTimeout: any;

  constructor(
    private msalWrapperService: MsalWrapperService,
    private readersRepository: ReadersRepositoryService,
    private subscriptionService: ChannelsSubscriptionsServiceService
  ) {
  }

  async ngOnInit() {
    this.msalWrapperService.isLoggedIn.subscribe(v => this.isLoggedIn = v);

    await this.readersRepository.GetAllAsync(r => {
      this.readersLoaded = true;
      this.readers = r;
    });
  }

  async onReaderClick(reader: ReaderViewModel) {
    this.selectedReader = reader;
    await this.readersRepository.GetReaderDetailsAsync(this.selectedReader.id, r => this.selectedReaderDetails = r);
  }

  async onChannelSubscribeClick(id: any) {
    this.subscriptionService.subscribeToChannel(id.toString())
      .then(() => this.showSubscribeNotification(true))
      .catch(() => this.showSubscribeNotification(false));
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
