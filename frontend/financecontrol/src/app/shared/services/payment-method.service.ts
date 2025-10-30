
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { GenericCrudService } from "src/app/core/services/generic-crud.service";
import { PaymentMethod } from "src/app/features/payment-method/models/payment-method";

@Injectable()
export class PaymentMethodService extends GenericCrudService<PaymentMethod> {

     constructor(protected override http: HttpClient) {
      super(http, 'PaymentMethod', 'paymentmethod');
  }
}