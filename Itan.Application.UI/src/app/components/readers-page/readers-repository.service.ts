import { Injectable } from '@angular/core';
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ReaderViewModel} from "../../../server/Itan/Core/GetAllReaders/ReaderViewModel";
import {ReaderDetailsViewModel} from "../../../server/Itan/Core/GetReader/ReaderDetailsViewModel";

@Injectable({
  providedIn: 'root'
})
export class ReadersRepositoryService {

  constructor(
    private msalWrapperService: MsalWrapperService,
    private http: HttpClient) { }

  async GetAllAsync(onSuccess) {
    let options = this.msalWrapperService.getOptionsHeaders();
    let url = `${environment.apiUrl}/api/readers`;
    this.http.get<ReaderViewModel[]>(url, options)
      .subscribe(res => onSuccess(res))
  }

  async GetReaderDetailsAsync(id:string, onSuccess,) {
    let options = this.msalWrapperService.getOptionsHeaders();
    let url = `${environment.apiUrl}/api/readers/${id}`;
    this.http.get<ReaderDetailsViewModel>(url, options)
      .subscribe(res => onSuccess(res))
  }
}
