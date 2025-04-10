import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationModule } from './features/navigation/navigation.module';
import { AccountModule } from './features/account/account.module';
import { LocalStorageUtils } from './core/utils/localstorage';


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
    NavigationModule
  ],
  providers: [LocalStorageUtils],
  bootstrap: [AppComponent]
})
export class AppModule { }
