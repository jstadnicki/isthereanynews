import {Component, OnInit} from "@angular/core";
import {Logger, CryptoUtils} from "msal";
import {HttpClient} from "@angular/common/http";
import {Client} from "@microsoft/microsoft-graph-client";

import {BroadcastService, MsalService} from "@azure/msal-angular";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.less"],
})
export class HeaderComponent implements OnInit {
  profile;
  account;
  authenticated: any;

  constructor(
    private broadcastService: BroadcastService,
    private authService: MsalService,
    private http: HttpClient
  ) {
  }

  loggedIn = false;

  ngOnInit(): void {
    this.broadcastService.subscribe("msal:loginSuccess", (e) => {
      this.checkAccount();
    });

    this.authService.handleRedirectCallback((authError, response) => {
      if (authError) {
        return;
      }
    });

    this.checkAccount();
  }

  logout() {
    this.authService.logout();
  }

  login() {
    const accessTokenRequest = {
      scopes: [
        'https://graph.microsoft.com/User.Read',
        "https://isthereanynewscodeblast.onmicrosoft.com/api/application_reader",
        "https://isthereanynewscodeblast.onmicrosoft.com/api/application_writer",
      ],
      clientId: "01805485-e711-4975-bbed-d10eb448d097",
      authority: "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup",
      redirectUri: "http://localhost:4200",
      postLogoutRedirectUri: 'http://localhost:4200/',
    };
    this.authService
      .loginPopup(accessTokenRequest);
  }

  private createUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  checkAccount() {
    this.account = this.authService.getAccount();
    this.loggedIn = !!this.account;
  }
}
