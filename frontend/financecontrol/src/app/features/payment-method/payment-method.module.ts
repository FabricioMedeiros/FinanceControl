import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { NgxSpinnerModule } from 'ngx-spinner';

import { PaymentMethodRoutingModule } from './payment-method-routing.module';
import { PaymentMethodComponent } from './payment-method.component';
import { PaymentMethodListComponent } from './components/payment-method-list/payment-method-list.component';
import { PaymentMethodFormComponent } from './components/payment-method-form/payment-method-form.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    PaymentMethodComponent,
    PaymentMethodListComponent,
    PaymentMethodFormComponent
  ],
  imports: [
    CommonModule,
    PaymentMethodRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    NgxSpinnerModule
  ]
})
export class PaymentMethodModule { }
