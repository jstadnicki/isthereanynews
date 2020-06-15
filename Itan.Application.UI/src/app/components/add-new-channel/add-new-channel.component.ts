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

  ngOnInit(): void {
  }

  async onSubmit(form: NgForm) {
    this.showSpinner = true;
    await this.addNewChannelRepositoryService
      .Save(this.url);
    this.showSpinner = false;
  }

}
