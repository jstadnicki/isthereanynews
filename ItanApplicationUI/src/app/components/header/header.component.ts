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
  }

  logout() {
    this.authService.logout();
  }

  login() {
    const accessTokenRequest = {
      scopes: [
        'https://graph.microsoft.com/User.Read',
        "https://isthereanynewscodeblast.onmicrosoft.com/05cd7635-e6f4-47c9-a5ce-8ec04368b297/application_reader",
        "https://isthereanynewscodeblast.onmicrosoft.com/05cd7635-e6f4-47c9-a5ce-8ec04368b297/application_writer",
      ],
      clientId: "f1ab593c-f0b4-44da-85dc-d89a457745a9",
      authority: "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup",
      redirectUri: "http://localhost:4200",
      postLogoutRedirectUri: 'http://localhost:4200/',
    };
    this.authService
      .loginPopup(accessTokenRequest);
  }

  checkAccount() {
    this.account = this.authService.getAccount();
    this.loggedIn = !!this.account;
  }
}
