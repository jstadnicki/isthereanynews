import {BrowserModule} from "@angular/platform-browser";
import {NgModule} from "@angular/core";
import {MsalModule, MsalGuard, MsalInterceptor} from "@azure/msal-angular";

import {AppComponent} from "./app.component";
import {HeaderComponent} from "./components/header/header.component";
import {FooterComponent} from "./components/footer/footer.component";
import {LandingPageComponent} from "./components/landing-page/landing-page.component";
import {RouterModule} from "@angular/router";
import {HomePageComponent} from "./components/home-page/home-page.component";
import {SettingsPageComponent} from "./components/settings-page/settings-page.component";
import {OwnerPageComponent} from "./components/owner-page/owner-page.component";
import {AdminPageComponent} from "./components/admin-page/admin-page.component";
import {ChannelsPageComponent} from "./components/channels-page/channels-page.component";
import {SubscriptionsPageComponent} from './components/subscriptions-page/subscriptions-page.component';

import {
  HTTP_INTERCEPTORS,
  HttpClientModule,
} from "@angular/common/http";
import {StripHtmlPipe} from "./components/channels-page/strip-html.pipe";

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
    StripHtmlPipe,
    SubscriptionsPageComponent
  ],

  imports: [
    HttpClientModule,
    BrowserModule,
    MsalModule.forRoot(
      {
        auth: {
          clientId: "01805485-e711-4975-bbed-d10eb448d097",
          authority:
            "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup",
          validateAuthority: false,
          redirectUri: "http://localhost:4200/",
          postLogoutRedirectUri: "http://localhost:4200/"
        },
        cache: {
          cacheLocation: "localStorage",
          storeAuthStateInCookie: true,
        },
      },
      {
        consentScopes: [
          "User.Read",
          "https://isthereanynewscodeblast.onmicrosoft.com/api/application_reader",
          "https://isthereanynewscodeblast.onmicrosoft.com/api/application_writer",
        ],
        unprotectedResources: [],
        protectedResourceMap: [
          ["https://graph.microsoft.com/me", ["User.Read"]],
        ],
      }
    ),
    RouterModule.forRoot([
      {path: "settings", component: SettingsPageComponent},
      {path: "owner", component: OwnerPageComponent},
      {path: "admin", component: AdminPageComponent},
      {path: "subscriptions", component: SubscriptionsPageComponent, canActivate: [MsalGuard]},
      {path: "channels", component: ChannelsPageComponent},
      {path: "**", component: HomePageComponent},
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
export class AppModule {
}
