import {Component, OnInit} from '@angular/core';
import {MsalWrapperService} from "../../service/msal-wrapper.service";
import {ReadersRepositoryService} from "./readers-repository.service";
import {ReaderViewModel} from "../../../server/Itan/Core/GetAllReaders/ReaderViewModel";
import {ReaderDetailsViewModel} from "../../../server/Itan/Core/GetReader/ReaderDetailsViewModel";

@Component({
  selector: 'app-readers-page',
  templateUrl: './readers-page.component.html',
  styleUrls: ['./readers-page.component.scss']
})
export class ReadersPageComponent implements OnInit {
  isLoggedIn: boolean;
  readers: ReaderViewModel[];
  selectedReader: ReaderViewModel;
  readersLoaded: boolean = false;
  selectedReaderDetails: ReaderDetailsViewModel;

  constructor(
    private msalWrapperService: MsalWrapperService,
    private readersRepository: ReadersRepositoryService
  ) {
  }

  async ngOnInit() {
    this.msalWrapperService.isLoggedIn.subscribe(v => this.isLoggedIn = v);

    await this.readersRepository.GetAllAsync(r => {
      this.readersLoaded = true;
      this.readers = r;
    });
  }

  async onReaderClick(reader: ReaderViewModel) {
    this.selectedReader = reader;

    await this.readersRepository.GetReaderDetailsAsync(this.selectedReader.id,r => this.selectedReaderDetails = r);

  }
}
