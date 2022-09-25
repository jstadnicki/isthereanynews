import {Injectable} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {Observable} from 'rxjs';
import {MsalWrapperService} from "./msal-wrapper.service";

@Injectable()
export class HttpJwtBearerInterceptor implements HttpInterceptor {

  constructor(private msalWrapper: MsalWrapperService) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let token = this.msalWrapper.getAccessToken();
    if (token != null) {
      const tokenizedReq = request.clone({headers: request.headers.set('Authorization', 'Bearer ' + token)});
    }
    return next.handle(request);
  }
}
