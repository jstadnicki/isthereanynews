import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BehaviorSubject, Subject} from "rxjs";
import {AuthenticationParameters} from "msal";
import {BroadcastService, MsalService} from "@azure/msal-angular";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class MsalWrapperService {
  private sessionId: string;
  isLoggedIn: Subject<boolean> = new BehaviorSubject<boolean>(false);
  private account;
  private sessionIdKeyName: string = "MsalWrapperService-uuid";
  private httpHeadersOptions: any;

  constructor(
    private broadcastService: BroadcastService,
    private authService: MsalService,
    private http: HttpClient) {

    this.broadcastService.subscribe("msal:loginSuccess", (e) => this.onMsalLoginSuccess(e));
    this.loginSilent();

  }

  private createUUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  logout() {
    this.authService.logout();
    this.authService.clearCacheForScope(this.sessionIdKeyName)
    localStorage.removeItem(this.sessionIdKeyName);

  }

  login() {
    const accessTokenRequest = this.createLoginRequest();
    this.authService
      .loginPopup(accessTokenRequest);
  }

  loginSilent() {
    let sessionId = localStorage.getItem(this.sessionIdKeyName);
    if (sessionId != null) {
      const accessTokenRequest = this.createLoginRequest();
      accessTokenRequest.sid = sessionId;
      this.authService.ssoSilent(accessTokenRequest)
        .then(() => this.checkAccount())
        .catch(() => localStorage.removeItem(this.sessionIdKeyName));
    }
  }

  private async createPersonAccount() {
    if (this.account.idToken.newUser === true) {
      let accessTokenRequest = this.createAccessRequest();
      const token = await this.authService.acquireTokenPopup(accessTokenRequest);
      const options = this.getOptions(token.accessToken);

      let body = {
        userId: this.account.accountIdentifier
      }

      this.http
        .post(`${environment.apiUrl}/api/users`, body, options)
        .subscribe(e => {
          console.log(e);
        });
    }
  }

  private getOptions(token: string) {
    let headers = new HttpHeaders();
    let setHeaders = headers
      .append("Authorization",`Bearer ${token}`);

    return {headers: setHeaders};
  }

  public async getOptionsHeaders():Promise<any> {
    if(this.httpHeadersOptions==null) {
      let accessTokenRequest = this.createAccessRequest();
      // const token = await this.authService.acquireTokenSilent(accessTokenRequest);
      const token = await this.authService.acquireTokenPopup(accessTokenRequest);
      this.httpHeadersOptions = this.getOptions(token.accessToken);
    }
    return this.httpHeadersOptions;
  }

  checkAccount(): void {
    this.account = this.authService.getAccount();
    this.sessionId = this.createUUID();
    localStorage.setItem(this.sessionIdKeyName, this.sessionId);
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
  private redirectUri: string = environment.homeUrl;

  private createLoginRequest(): AuthenticationParameters {
    return {
      scopes: [
        'https://graph.microsoft.com/User.Read',
      ],
      authority: this.authority,
      redirectUri: this.redirectUri,
    };
  }



  private createAccessRequest(): AuthenticationParameters {
    return {
      scopes: [
        "https://isthereanynewscodeblast.onmicrosoft.com/api/application-reader",
        "https://isthereanynewscodeblast.onmicrosoft.com/api/application-writer",
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


}
