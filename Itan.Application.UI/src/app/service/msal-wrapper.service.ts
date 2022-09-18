import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Subject } from "rxjs";
import { environment } from "../../environments/environment";
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import {AuthenticationResult, EventMessage, EventType, InteractionStatus} from "@azure/msal-browser";
import { filter } from "rxjs/operators";

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
  private _authentication: AuthenticationResult;

  constructor(
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private http: HttpClient) {

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
    this._account = this.authService.instance.getAllAccounts()[0];
    if(this._account!=null){
      this.authService.acquireTokenSilent({
        scopes:['profile', 'https://isthereanynewscodeblast.onmicrosoft.com/api/application-reader'],
        account:this._account
      }).subscribe(e=> {
        this._authentication = e;
      })
      this._userName.next(this._account?.name);
      this._isLoggedIn.next(this._account != null);
    }
  }

  logout() {
    this.authService.logout();
  }

  login() {
    this.authService.loginRedirect(
      {
        scopes:['profile', 'https://isthereanynewscodeblast.onmicrosoft.com/api/application-reader']
      }
    );
  }

  private getOptions(token: string): HttpHeaders {
    let headers = new HttpHeaders();
    let setHeaders = headers
      .append("Authorization", `Bearer ${token}`)
      .append("Content-Type", "application/json");

    return setHeaders;
  }

  public getOptionsHeaders() {
    return { headers: this.getOptions(this._authentication.accessToken) };
  }
}
