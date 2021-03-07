import {Injectable} from '@angular/core';
import {MsalWrapperService} from "./../../service/msal-wrapper.service";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ReaderSettings} from "./reader.settings";
import {UpdatedNews} from "./updated.news";
import {SquashUpdate} from "./squash.update";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SettingsServiceService {

  constructor(
    private msalWrapperService: MsalWrapperService,
    private http: HttpClient,

  ) {
  }

  async updateShowsUpdated(showUpdatedNews: UpdatedNews) {
    const options = await this.msalWrapperService.getOptionsHeaders();


    const body = {
      showUpdatedNews: showUpdatedNews
    };

    return this.http.patch(`${environment.apiUrl}/api/settings/show-updated-news`, body, options);
  }

  async squashNewsUpdates(squashUpdate: SquashUpdate) {
    const options = await this.msalWrapperService.getOptionsHeaders();

    const body = {
      squashUpdate:squashUpdate
    };

    return this.http.patch(`${environment.apiUrl}/api/settings/squash-news-updates`, body, options);
  }

  async getReaderSettings():Promise<Observable<ReaderSettings>> {
    const options = await this.msalWrapperService.getOptionsHeaders();
    return this.http.get<ReaderSettings>(`${environment.apiUrl}/api/settings/reader`, options);
  }
}
