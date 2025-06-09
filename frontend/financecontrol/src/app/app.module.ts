import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDatepickerModule, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale, ptBrLocale } from 'ngx-bootstrap/chronos';

import { CurrencyMaskModule } from 'ng2-currency-mask';
import { CurrencyMaskConfig, CURRENCY_MASK_CONFIG } from 'ng2-currency-mask';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationModule } from './features/navigation/navigation.module';
import { AccountModule } from './features/account/account.module';
import { LocalStorageUtils } from './core/utils/localstorage';
import { ErrorInterceptor } from './core/interceptors/error.handler.service';
import { NavigationService } from './core/services/navigation.service';
import { SharedModule } from './shared/shared.module';
import { CategoryModule } from './features/category/category.module';

export const CustomCurrencyMaskConfig: CurrencyMaskConfig = {
  align: 'right',
  allowNegative: true,
  decimal: ',',
  precision: 2,
  prefix: 'R$ ',
  suffix: '',
  thousands: '.'
};

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
];

defineLocale('pt-br', ptBrLocale);

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
    NgxSpinnerModule.forRoot(),
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    CurrencyMaskModule,
    AccountModule,
    NavigationModule,
    SharedModule,
    CategoryModule
  ],
  providers: [
    LocalStorageUtils,
    httpInterceptorProviders,
    NavigationService,
    { provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(private localeService: BsLocaleService) { 
    this.localeService.use('pt-br');
  }
 }
