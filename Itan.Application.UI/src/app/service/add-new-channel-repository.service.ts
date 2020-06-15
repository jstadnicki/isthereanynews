import {Injectable} from '@angular/core';
import {MsalWrapperService} from "./msal-wrapper.service";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

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
    const options = await this.msalWrapperService.getOptionsWriteHeaders();

    const body = {
      url: url
    };

    return this.http
      .post(`https://localhost:5001/api/channels`, body, options);
  }
}
