import {Injectable} from '@angular/core';
import {BroadcastService, MsalService} from "@azure/msal-angular";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BehaviorSubject, Subject} from "rxjs";
import {AuthenticationParameters} from "msal";

@Injectable({
  providedIn: 'root'
})
export class MsalWrapperService {
  private sessionId: string;
  isLoggedIn: Subject<boolean> = new BehaviorSubject<boolean>(false);
  private account;

  constructor(
    private broadcastService: BroadcastService,
    private authService: MsalService,
    private http: HttpClient) {

    this.broadcastService.subscribe("msal:loginSuccess", (e) => this.onMsalLoginSuccess(e));
    this.checkAccount();

  }

  private createUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  logout() {
    this.authService.logout();
  }

  login() {
    const accessTokenRequest = this.createLoginRequest();
    this.authService
      .loginPopup(accessTokenRequest);
  }

  private async createPersonAccount() {
    let accessTokenRequest = this.createWriteAccessRequest();
    const token = await this.authService.acquireTokenSilent(accessTokenRequest);
    const options = this.getOptions(token.accessToken);

    let body = {
      userId: this.account.accountIdentifier
    }

    this.http
      .post("https://localhost:5001/api/users", body, options)
      .subscribe(e => {
        console.log(e);
      });
  }

  private getOptions(token: string) {
    return {
      headers: new HttpHeaders({Authorization: `Bearer ${token}`}),
    };
  }

  public async getOptionsWriteHeaders() {
    let accessTokenRequest = this.createWriteAccessRequest();
    const token = await this.authService.acquireTokenSilent(accessTokenRequest);
    return this.getOptions(token.accessToken);
  }

  checkAccount(): void {
    this.account = this.authService.getAccount();
    this.sessionId = this.createUUID();
    this.isLoggedIn.next(!!this.account);
  }

  getUserName(): string {
    return this.account.name;
  }

  onMsalLoginSuccess(e): void {
    this.checkAccount();
    this.createPersonAccount();
  }

  private authority: string = "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup";
  private redirectUri: string = "http://localhost:4200";

  private createLoginRequest(): AuthenticationParameters {
    return {
      scopes: [
        'https://graph.microsoft.com/User.Read',
      ],
      authority: this.authority,
      redirectUri: this.redirectUri,
    };
  }

  private createWriteAccessRequest(): AuthenticationParameters {
    return {
      scopes: [
        "https://isthereanynewscodeblast.onmicrosoft.com/api/application_writer",
      ],
      authority: this.authority,
      redirectUri: this.redirectUri,
      account: this.account,
      sid: this.sessionId
    };
  }

  private createReadAccessRequest(): AuthenticationParameters {
    return {
      scopes: [
        "https://isthereanynewscodeblast.onmicrosoft.com/api/application_writer",
      ],
      authority: this.authority,
      redirectUri: this.redirectUri,
      account: this.account,
      sid: this.sessionId
    };
  }

  getAccountId() {
    return this.account.accountIdentifier;
  }

  public async getOptionsReadHeaders() {
    let accessTokenRequest = this.createReadAccessRequest();
    const token = await this.authService.acquireTokenSilent(accessTokenRequest);
    return this.getOptions(token.accessToken);
  }
}
