import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TransactionRoutingModule } from './transaction-routing.module';
import { TransactionComponent } from './transaction.component';
import { TransactionListComponent } from './components/transaction-list/transaction-list.component';
import { TransactionFormComponent } from './components/transaction-form/transaction-form.component';


@NgModule({
  declarations: [
    TransactionComponent,
    TransactionListComponent,
    TransactionFormComponent
  ],
  imports: [
    CommonModule,
    TransactionRoutingModule
  ]
})
export class TransactionModule { }
