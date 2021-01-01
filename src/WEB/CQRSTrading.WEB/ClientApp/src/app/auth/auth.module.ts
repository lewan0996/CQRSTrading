import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MsalAngularConfiguration, MsalInterceptor, MsalModule, MsalService, MSAL_CONFIG, MSAL_CONFIG_ANGULAR } from '@azure/msal-angular';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { Configuration } from 'msal';
import { msalConfig, msalAngularConfig } from './msal-config';

function MSALConfigFactory(): Configuration {
  return msalConfig;
}

function MSALAngularConfigFactory(): MsalAngularConfiguration {
  return msalAngularConfig;
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MsalModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_CONFIG,
      useFactory: MSALConfigFactory
    },
    {
      provide: MSAL_CONFIG_ANGULAR,
      useFactory: MSALAngularConfigFactory
    },
    MsalService
  ]
})
export class AuthModule { }
