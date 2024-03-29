import {BrowserModule} from "@angular/platform-browser";
import {NgModule} from "@angular/core";
import {MsalModule, MsalGuard, MsalRedirectComponent, MsalInterceptor} from "@azure/msal-angular";

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
import {StripHtmlPipe} from "./components/channels-page/strip-html.pipe";
import {AddNewChannelComponent} from './components/add-new-channel/add-new-channel.component';
import {PrivacyPageComponent} from './components/privacy-page/privacy-page.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule,} from "@angular/common/http";
import {LockerComponent} from './components/locker/locker.component';
import {environment} from "../environments/environment";
import {DeleteAccountComponent} from './components/delete-account/delete-account.component';
import {ReadersPageComponent} from './components/readers-page/readers-page.component';
import {LandingPageNewsComponent} from './components/landing-page-news/landing-page-news.component';
import {InteractionType, PublicClientApplication} from "@azure/msal-browser";
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
    SubscriptionsPageComponent,
    AddNewChannelComponent,
    LockerComponent,
    PrivacyPageComponent,
    DeleteAccountComponent,
    ReadersPageComponent,
    LandingPageNewsComponent,
  ],

  imports: [
    HttpClientModule,
    BrowserModule,
    FormsModule,
    MsalModule.forRoot(new PublicClientApplication({
      auth: {
        clientId: '9181bdde-959f-42a6-a253-b10a6f05d883',
        authority: "https://isthereanynewscodeblast.b2clogin.com/isthereanynewscodeblast.onmicrosoft.com/B2C_1_itansignup",
        knownAuthorities: ['isthereanynewscodeblast.b2clogin.com'],
        redirectUri: environment.homeUrl,
      },
      cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: isIE,
      }
    }), {
      interactionType: InteractionType.Redirect,
      authRequest: {
        scopes: ['user.read', 'User.Read.All', 'profile']
      }
    }, {
      interactionType: InteractionType.Redirect,
      protectedResourceMap: new Map([
        ['http://localhost:5000/api/subscription', ['profile', 'https://isthereanynewscodeblast.onmicrosoft.com/api/application-reader']]
      ])
    }),
    RouterModule.forRoot([
      {path: "settings", component: SettingsPageComponent},
      {path: "owner", component: OwnerPageComponent},
      {path: "admin", component: AdminPageComponent},
      {path: "subscriptions", component: SubscriptionsPageComponent, canActivate: [MsalGuard]},
      {path: "channels", component: ChannelsPageComponent},
      {path: "privacy", component: PrivacyPageComponent},
      {path: "readers", component: ReadersPageComponent},
      {path: "delete-account", component: DeleteAccountComponent},
      {path: "**", component: HomePageComponent}
    ], {relativeLinkResolution: 'legacy'}),
    ReactiveFormsModule,
  ],
  // providers: [
  //   {
  //     provide: HTTP_INTERCEPTORS,
  //     useClass: HttpJwtBearerInterceptor,
  //     multi: true,
  //   },
  //   MsalGuard
  // ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true,
    },
    MsalGuard
  ],
  bootstrap: [AppComponent, MsalRedirectComponent],
})
export class AppModule {
}
