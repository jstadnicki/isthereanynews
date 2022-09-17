import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { environment } from "../../environments/environment";
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { EventMessage, EventType, InteractionStatus, SilentRequest } from "@azure/msal-browser";
import { filter, tap } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class MsalWrapperService {

  private authority: string = "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup";
  private redirectUri: string = environment.homeUrl;

  private _isLoggedIn = new BehaviorSubject(false);
  public isLoggedIn$ = this._isLoggedIn.asObservable();

  private _account: any = null;
  private accessToken: string;

  private _userName: Subject<string> = new Subject();
  public userName$ = this._userName.asObservable();

  private _accountIndentifier: Subject<string> = new Subject();
  public accountIndentifier$ = this._accountIndentifier.asObservable();

  constructor(
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private http: HttpClient) {

    this.msalBroadcastService.msalSubject$
      .pipe(
        filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS),
      )
      .subscribe((result: EventMessage) => {
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
    this._account = this.authService.instance.getAllAccounts()[0];

    this._userName.next(this._account?.name);
    this._accountIndentifier.next(this._account?.localAccountId);

    this._isLoggedIn.next(this._account != null);

  }

  logout() {
    this.authService.logout();
    sessionStorage.clear();
  }

  login() {
    this.authService.loginRedirect();
  }

  private getOptions(token: string): HttpHeaders {
    let headers = new HttpHeaders();
    let setHeaders = headers
      .append("Authorization", `Bearer ${token}`)
      .append("Content-Type", "application/json");

    return setHeaders;
  }

  public getOptionsHeaders() {
    return { headers: this.getOptions(this.accessToken) };
  }


}
