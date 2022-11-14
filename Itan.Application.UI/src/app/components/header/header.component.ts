import {Component, OnInit} from "@angular/core";
import {MsalWrapperService} from "../../service/msal-wrapper.service";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.less"],
})
export class HeaderComponent implements OnInit {
  constructor(
    public msalWrapper: MsalWrapperService
  ) {
  }

  loggedIn: boolean = false;
  isBurgerMenuActive: boolean = false;
  public username: string;

  ngOnInit(): void {
    this.msalWrapper.isLoggedIn$.subscribe(s => {
      this.loggedIn = s;
    });

    this.msalWrapper._account$.subscribe(s => {
        this.username = s?.name;
      }
    )
  }

  logout(): void {
    this.msalWrapper.logout();
  }

  login(): void {
    this.msalWrapper.login();
  }

  onBurgerClick() {
    this.isBurgerMenuActive = !this.isBurgerMenuActive;
  }
}
