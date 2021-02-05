import { Component, OnInit } from '@angular/core';
import {DeleteAccountService} from "../../service/delete-account.service";

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrls: ['./delete-account.component.scss']
})
export class DeleteAccountComponent implements OnInit {

  constructor(private deleteAccountService: DeleteAccountService) { }

  ngOnInit(): void {
  }

  promptForDeletionAcknolwedge() {

    if(confirm("Are you sure you want to delete account")){
      this.deleteAccount();
    }
  }

  deleteAccount() {
    this.deleteAccountService.run();
  }
}
