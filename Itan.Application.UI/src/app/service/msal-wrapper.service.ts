import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BehaviorSubject, Subject} from "rxjs";
import {AuthenticationParameters, AuthError, AuthResponse} from "msal";
import {BroadcastService, MsalService} from "@azure/msal-angular";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class MsalWrapperService {
  private sessionId: string;
  isLoggedIn: Subject<boolean> = new BehaviorSubject<boolean>(false);
  private account: any = null;
  private sessionIdKeyName: string = "MsalWrapperService-uuid";
  private tokenKeyName: string = "MsalWrapperService-accessToken";
  private accessToken: string;

  constructor(
    private broadcastService: BroadcastService,
    private authService: MsalService,
    private http: HttpClient) {

    this.authService.handleRedirectCallback(c => this.onCallback(c), e => this.onError(e))
    this.broadcastService.subscribe("msal:loginSuccess", (e) => this.onMsalLoginSuccess(e));

    this.restoreUser();
  }

  private onCallback(response: AuthResponse) {
    this.accessToken = response.accessToken;
    this.account = response.account;
    if (response.accessToken == null || response.accessToken == undefined) {
      let accessTokenRequest = this.createAccessRequest();
      this.authService.acquireTokenRedirect(accessTokenRequest)
    } else {
      if (this.account.idToken.newUser === true) {
        this.createPersonAccount();
      } else {
        this.migrateAccount();
      }
      this.completeLogin()
    }
  }

  private completeLogin() {
    this.sessionId = this.createUUID();
    sessionStorage.setItem(this.sessionIdKeyName, this.sessionId);
    sessionStorage.setItem(this.tokenKeyName, this.accessToken);
    this.isLoggedIn.next(!!this.account);
  }

  private restoreUser() {
    let sessionId = sessionStorage.getItem(this.sessionIdKeyName);
    let accessToken = sessionStorage.getItem(this.tokenKeyName);

    if(sessionId == null || sessionId.length==0){
      return;
    }

    if(accessToken == null || accessToken.length==0){
      return null;
    }

    this.account = this.authService.getAccount()
    this.isLoggedIn.next(!!this.account);
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
    sessionStorage.clear();
  }

  login() {
    const accessTokenRequest = this.createLoginRequest();
    this.authService.loginRedirect(accessTokenRequest);
  }

  private async createPersonAccount() {
    const options = this.getOptionsHeaders();

    let body = {
      userId: this.account.accountIdentifier
    }

    this.http
      .post(`${environment.apiUrl}/api/users`, body, options)
      .subscribe();
  }

  private migrateAccount() {
    const options = this.getOptionsHeaders();
    var body = {};
    this.http
      .post(`${environment.apiUrl}/api/users/migrate`, body, options)
      .subscribe();
  }

  private getOptions(token: string): HttpHeaders {
    let headers = new HttpHeaders();
    let setHeaders = headers
      .append("Authorization", `Bearer ${token}`)
      .append("Content-Type", "application/json");

    return setHeaders;
  }

  public getOptionsHeaders() {
    return {headers: this.getOptions(this.accessToken)};
  }

  getUserName(): string {
    return this.account.name;
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


  private onError(e: AuthError) {

  }

  private onMsalLoginSuccess(e: any) {

  }
}
