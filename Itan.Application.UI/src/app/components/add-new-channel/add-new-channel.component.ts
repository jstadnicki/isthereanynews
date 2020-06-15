import {Component, OnInit} from '@angular/core';
import {NgForm} from "@angular/forms";
import {AddNewChannelRepositoryService} from "../../service/add-new-channel-repository.service";

@Component({
  selector: 'app-add-new-channel',
  templateUrl: './add-new-channel.component.html',
  styleUrls: ['./add-new-channel.component.scss']
})
export class AddNewChannelComponent implements OnInit {

  showSpinner: boolean = false;

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
    .subscribe(s=>this.showSuccess(),e=>this.showError());
    this.showSpinner = false;
  }

  async showSuccess() {
    this.displaySuccess = true;
    setTimeout(() => {
      this.displaySuccess = false;
    }, 3000);
  };

  async showError() {
    this.displayError = true;
    setTimeout(() => {
      this.displayError = false;
    }, 3000);
  };

}
