import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';
import { PaymentMethod } from '../../models/payment-method';
import { PaymentMethodService } from 'src/app/shared/services/payment-method.service';

@Component({
  selector: 'app-payment-method-form',
  templateUrl: './payment-method-form.component.html',
  styleUrls: ['./payment-method-form.component.css']
})
export class PaymentMethodFormComponent extends BaseFormComponent<PaymentMethod> implements OnInit {

  constructor(
    fb: FormBuilder,
    router: Router,
    route: ActivatedRoute,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    private paymentMethodService: PaymentMethodService
  ) {
    super(fb, router, route, toastr, spinner);

    this.validationMessages = {
      name: { required: 'Informe o nome' }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();

    const resolvedData = this.route.snapshot.data['paymentMethod'];

    if (resolvedData) {
      this.initializeForm(this.initializeForm({
        data: {
          ...resolvedData?.data,
        }
      }));
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      name: ['', [Validators.required]]
    });
  }

  savePaymentMethod() {
    if (this.form.dirty && this.form.valid) {
      this.changesSaved = true;

      const paymentMethodData = Object.fromEntries(
        Object.entries(this.form.value).filter(([key, value]) => key !== 'id' || (value && value !== ''))
      ) as unknown as PaymentMethod;

      if (this.isEditMode) {
        this.paymentMethodService.updatePaymentMethod(paymentMethodData).subscribe({
          next: () => this.processSuccess('Forma de Pagamento atualizada com sucesso!', '/payment-method/list'),
          error: (error) => this.processFail(error)
        });
      } else {
        this.paymentMethodService.registerPaymentMethod(paymentMethodData).subscribe({
          next: () => this.processSuccess('Forma de Pagamento cadastrada com sucesso!', '/payment-method/list'),
          error: (error) => {
            this.processFail(error);
          }
        });
      }
    }
  }
}
