import {Component, OnInit} from '@angular/core';
import {NgForm} from "@angular/forms";
import {AddNewChannelRepositoryService} from "./add-new-channel-repository.service";

@Component({
  selector: 'app-add-new-channel',
  templateUrl: './add-new-channel.component.html',
  styleUrls: ['./add-new-channel.component.scss']
})
export class AddNewChannelComponent implements OnInit {

  showSpinner: boolean = false;
  successMessage: string='';

  constructor(
    private addNewChannelRepositoryService: AddNewChannelRepositoryService
  ) {
  }

  url: string;
  displaySuccess: boolean = false;
  displayError: boolean = false;

  ngOnInit(): void {
  }

  async onSubmit(form: NgForm) {
    this.showSpinner = true;
    (await this.addNewChannelRepositoryService.Save(this.url))
      .subscribe(s => {
          this.showSuccess(s)
        },
        e => this.showError());
    this.showSpinner = false;
  }

  async showSuccess(s) {
    if(s.channelCreateRequestResultType == 2){
      this.successMessage = `Channel already exists, find it by ${s.channelName}`;
    }
    if(s.channelCreateRequestResultType == 1){
      this.successMessage = `Channel created, find it by ${s.channelName}. Please give ITAN some time to parse it (refresh maybe be required - sorry)`;
    }
    this.displaySuccess = true;
    setTimeout(() => {
      this.displaySuccess = false;
    }, 15000);
  };

  async showError() {
    this.displayError = true;
    setTimeout(() => {
      this.displayError = false;
    }, 5000);
  };

}
