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
      this.entity = Object.assign({}, this.form.value);

      const request = this.paymentMethodService.save(this.entity);

      request.subscribe({
        next: () => {
          const msg = this.isEditMode
            ? 'Forma de Pagamento atualizada com sucesso!'
            : 'Forma de Pagamento cadastrada com sucesso!';
          this.processSuccess(msg, '/payment-method/list');
        },
        error: err => this.processFail(err)
      });
    }
  }
}
