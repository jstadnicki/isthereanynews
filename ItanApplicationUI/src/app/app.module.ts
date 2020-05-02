import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { MsalModule, MsalGuard, MsalInterceptor } from "@azure/msal-angular";

import { AppComponent } from "./app.component";
import { HeaderComponent } from "./components/header/header.component";
import { FooterComponent } from "./components/footer/footer.component";
import { LandingPageComponent } from "./components/landing-page/landing-page.component";
import { RouterModule } from "@angular/router";
import { HomePageComponent } from "./components/home-page/home-page.component";
import { SettingsPageComponent } from "./components/settings-page/settings-page.component";
import { OwnerPageComponent } from "./components/owner-page/owner-page.component";
import { AdminPageComponent } from "./components/admin-page/admin-page.component";
import { ChannelsPageComponent } from "./components/channels-page/channels-page.component";

import {
  HTTP_INTERCEPTORS,
  HttpClientModule,
} from "@angular/common/http";
const isIE =
  window.navigator.userAgent.indexOf("MSIE ") > -1 ||
  window.navigator.userAgent.indexOf("Trident/") > -1;

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    LandingPageComponent,
    HomePageComponent,
    SettingsPageComponent,
    OwnerPageComponent,
    AdminPageComponent,
    ChannelsPageComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    MsalModule.forRoot(
      {
        auth: {
          clientId: "f1ab593c-f0b4-44da-85dc-d89a457745a9",
          authority:
            "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup",
          validateAuthority: false,
          redirectUri:"http://localhost:4200/",
          postLogoutRedirectUri:"http://localhost:4200/"
        },
        cache: {
          cacheLocation: "localStorage",
          storeAuthStateInCookie: true,
        },
      },
      {
        consentScopes: [
          "User.Read",
          "https://isthereanynewscodeblast.onmicrosoft.com/05cd7635-e6f4-47c9-a5ce-8ec04368b297/application_reader",
          "https://isthereanynewscodeblast.onmicrosoft.com/05cd7635-e6f4-47c9-a5ce-8ec04368b297/application_writer",
        ],
        unprotectedResources: [],
        protectedResourceMap: [
          ["https://graph.microsoft.com/me", ["User.Read"]],
        ],
      }
    ),
    RouterModule.forRoot([
      { path: "settings", component: SettingsPageComponent },
      { path: "owner", component: OwnerPageComponent },
      { path: "admin", component: AdminPageComponent },
      {
        path: "channels",
        component: ChannelsPageComponent,
        canActivate: [MsalGuard],
      },
      { path: "**", component: HomePageComponent },
    ]),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
