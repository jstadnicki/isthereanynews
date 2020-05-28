import {Component, OnInit} from "@angular/core";
import {MsalWrapperService} from "../../service/msal-wrapper.service";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.less"],
})
export class HeaderComponent implements OnInit {
  constructor(
    private msalWrapper: MsalWrapperService
  ) {
    this.msalWrapper.isLoggedIn.subscribe(s=>this.loggedIn = s);
  }

  loggedIn: boolean = false;

  ngOnInit(): void {
  }

  logout(): void {
    this.msalWrapper.logout();
  }

  login(): void {
    this.msalWrapper.login();
  }

  getUserName(): string {
    return this.msalWrapper.getUserName();
  }
}
