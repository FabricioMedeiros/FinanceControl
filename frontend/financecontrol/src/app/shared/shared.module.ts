import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { CurrencyFormatPipe } from './pipes/currency-format.pipe';

import { CategoryService } from './services/category.service';
import { PaymentMethodService } from './services/payment-method.service';
import { TransactionService } from './services/transaction.service';

@NgModule({
  declarations: [
    SearchBarComponent,
    PaginationComponent,
    CurrencyFormatPipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports:[
    PaginationComponent,
    SearchBarComponent,
    CurrencyFormatPipe
  ],
  providers: [CategoryService, PaymentMethodService, TransactionService]
})
export class SharedModule { }
