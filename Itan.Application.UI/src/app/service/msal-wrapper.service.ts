import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BehaviorSubject, Subject} from "rxjs";
import {environment} from "../../environments/environment";
import {MsalBroadcastService, MsalService} from '@azure/msal-angular';
import {AccountInfo, AuthenticationResult, EventMessage, EventType, InteractionStatus} from "@azure/msal-browser";
import {catchError, filter, tap} from "rxjs/operators";
import {Logger, CryptoUtils} from 'msal';
import {LoggerOptions} from "@azure/msal-common/dist/config/ClientConfiguration";
import {LogLevel} from "@azure/msal-common/dist/logger/Logger";


@Injectable({
  providedIn: 'root'
})
export class MsalWrapperService {

  private authority: string = "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup";

  private _isLoggedIn = new BehaviorSubject(false);
  public isLoggedIn$ = this._isLoggedIn.asObservable();

  private _account = new BehaviorSubject<AccountInfo>(null);
  public _account$ = this._account.asObservable()
  private _accessToken: string | undefined;

  constructor(
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private http: HttpClient) {

    this.authService.handleRedirectObservable().subscribe(r => {
      if (r?.account != null) {
        this._account.next(r?.account);
        this._isLoggedIn.next(r.account != null);
        this._accessToken = r?.accessToken;
      }
    });

    this.msalBroadcastService.msalSubject$
      .pipe(
        filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS),
      )
      .subscribe((e: EventMessage) => {
        this.restoreUser();
      });

    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None)
      )
      .subscribe((e) => {
        this.restoreUser();
      })
  }

  private restoreUser() {
    let account = this.authService.instance.getAllAccounts()[0];
    if (account != null) {
      this.authService.acquireTokenSilent({
        scopes: ['profile', 'https://isthereanynewscodeblast.onmicrosoft.com/api/application-reader'],
        account: account
      })
        .pipe(
          tap(result => {
            this._accessToken = result.accessToken;
            this._account.next(result?.account);
            this._isLoggedIn.next(result.account != null);
          }),
          catchError(ex => {
            return this.authService.acquireTokenRedirect({
              authority: this.authority,
              scopes: ['profile', 'https://isthereanynewscodeblast.onmicrosoft.com/api/application-reader']
            })
          }))
        .subscribe()
    }
  }



  public getOptionsHeaders() {
    return {headers: this.getOptions(this._accessToken)};
  }

  private getOptions(token: string):
    HttpHeaders {
    let headers = new HttpHeaders();
    let setHeaders = headers
      .append("Authorization", `Bearer ${token}`)
      .append("Content-Type", "application/json");

    return setHeaders;
  }

  login() {
    this.authService.loginRedirect(
      {
        scopes: ['profile', 'https://isthereanynewscodeblast.onmicrosoft.com/api/application-reader']
      }
    );
  }

  logout() {
    this.authService.logout();
  }
}
