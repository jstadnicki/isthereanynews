import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AddNewChannelRepositoryService {

  constructor(
    private http: HttpClient,
    private msalWrapperService: MsalWrapperService
  ) {
  }

  async Save(url: string): Promise<Observable<Object>> {
    const options = this.msalWrapperService.getOptionsHeaders();

    const body = {
      url: url
    };

    return this.http
      .post(`${environment.apiUrl}/api/channels`, body, options);
  }
}
