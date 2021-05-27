import {Component, OnInit} from '@angular/core';
import {NgForm} from "@angular/forms";
import {catchError, tap} from "rxjs/operators";
import {HttpClient} from "@angular/common/http";
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-add-new-channel',
  templateUrl: './add-new-channel.component.html',
  styleUrls: ['./add-new-channel.component.scss']
})
export class AddNewChannelComponent implements OnInit {

  showSpinner: boolean = false;
  successMessage: string = '';

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) {
  }

  url: string;
  displaySuccess: boolean = false;
  displayError: boolean = false;

  ngOnInit(): void {
  }

  async onSubmit(form: NgForm) {
    this.showSpinner = true;
    const options = this.msalWrapperService.getOptionsHeaders();

    const body = {
      url: this.url
    };
    this
      .http
      .post(`${environment.apiUrl}/api/channels`, body, options)
      .pipe(
        tap(s => this.showSuccess(s)),
        tap(() => this.showSpinner = false),
        catchError(e => this.showError()),
      )
      .subscribe();
  }

  async showSuccess(s) {
    if (s.channelCreateRequestResultType == 2) {
      this.successMessage = `Channel already exists, find it by ${s.channelName}`;
    }
    if (s.channelCreateRequestResultType == 1) {
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
