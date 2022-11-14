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
    return next.handle(request);
  }
}
