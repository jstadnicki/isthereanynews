import {Component, OnInit} from "@angular/core";
import {Logger, CryptoUtils} from "msal";
import {HttpClient, HttpHeaders} from "@angular/common/http";
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
  sessionId = this.createUUID();

  private createUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  ngOnInit(): void {
    this.broadcastService.subscribe("msal:loginSuccess", (e) => {
      this.createPersonAccount();
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

  async createPersonAccount() {
    const a = this.authService.getAccount();

    const accessTokenRequest = {
      scopes: [
        "https://isthereanynewscodeblast.onmicrosoft.com/api/application_writer",
      ],
      clientId: "01805485-e711-4975-bbed-d10eb448d097",
      authority:
        "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup",
      redirectUri: "http://localhost:4200",
      account: a,
      sid: this.sessionId
    };

    const x = await this.authService.acquireTokenSilent(accessTokenRequest);

    const o = this.getOptions(x.accessToken);

    let body = {
      userId: a.accountIdentifier
    }

    this.http
      .post("https://localhost:5001/api/users", body, o)
      .subscribe(e => {
        console.log(e);
      });

  }

  private getOptions(token: string) {
    return {
      headers: new HttpHeaders({Authorization: `Bearer ${token}`}),
    };
  }

  checkAccount() {
    this.account = this.authService.getAccount();
    this.loggedIn = !!this.account;
  }
}
