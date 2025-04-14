import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationModule } from './features/navigation/navigation.module';
import { AccountModule } from './features/account/account.module';
import { LocalStorageUtils } from './core/utils/localstorage';
import { ErrorInterceptor } from './core/interceptors/error.handler.service';
import { NavigationService } from './core/services/navigation.service';
import { SharedModule } from './shared/shared.module';

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({ toastClass: 'ngx-toastr custom-toast', preventDuplicates: true }),
    AccountModule,
    NavigationModule,
    SharedModule
  ],
  providers: [LocalStorageUtils, httpInterceptorProviders, NavigationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
