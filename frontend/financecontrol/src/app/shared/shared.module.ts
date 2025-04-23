import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CategoryService } from './services/category.service';
import { PaymentMethodService } from './services/payment-method.service';

@NgModule({
  declarations: [
    SearchBarComponent,
    PaginationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports:[
    PaginationComponent,
    SearchBarComponent
  ],
  providers: [CategoryService, PaymentMethodService]
})
export class SharedModule { }
