import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { CurrencyFormatPipe } from './pipes/currency-format.pipe';

import { CategoryService } from './services/category.service';
import { PaymentMethodService } from './services/payment-method.service';
import { TransactionService } from './services/transaction.service';
import { ButtonScrollTopComponent } from './components/button-scroll-top/button-scroll-top.component';

@NgModule({
  declarations: [
    SearchBarComponent,
    PaginationComponent,
    CurrencyFormatPipe,
    ButtonScrollTopComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports:[
    PaginationComponent,
    SearchBarComponent,
    CurrencyFormatPipe,
    ButtonScrollTopComponent
  ],
  providers: [CategoryService, PaymentMethodService, TransactionService]
})
export class SharedModule { }
