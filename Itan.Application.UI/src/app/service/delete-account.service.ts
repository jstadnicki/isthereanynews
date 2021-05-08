import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MsalWrapperService} from "./msal-wrapper.service";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class DeleteAccountService {

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) { }

  async run() {
    var options = this.msalWrapperService.getOptionsHeaders();
    this.http
      .delete(`${environment.apiUrl}/api/delete-account`, options)
      .subscribe((r) => {
        alert('done');
        this.msalWrapperService.logout();
      });
  }
}
