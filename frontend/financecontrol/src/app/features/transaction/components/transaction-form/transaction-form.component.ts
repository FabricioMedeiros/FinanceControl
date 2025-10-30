import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';
import { Transaction } from '../../models/transaction';
import { Category } from '../../models/category';
import { PaymentMethod } from '../../models/payment-method';
import { TransactionService } from 'src/app/shared/services/transaction.service';
import { CategoryService } from 'src/app/shared/services/category.service';
import { PaymentMethodService } from 'src/app/shared/services/payment-method.service';

@Component({
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.css']
})
export class TransactionFormComponent extends BaseFormComponent<Transaction> implements OnInit {
  categories: Category[] = [];
  paymentMethods: PaymentMethod[] = [];

  constructor(
    fb: FormBuilder,
    router: Router,
    route: ActivatedRoute,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    private transactionService: TransactionService,
    private categoryService: CategoryService,
    private paymentMethodService: PaymentMethodService,
  ) {
    super(fb, router, route, toastr, spinner);

    this.validationMessages = {
      date: { required: 'Informe a data' },
      amount: { required: 'Informe o valor' },
      categoryId: { required: 'Selecione a categoria' },
      paymentMethodId: { required: 'Informe a forma de pagamento' }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();

    const resolvedData = this.route.snapshot.data['transaction'];

    if (resolvedData) {
      this.initializeForm({
        data: {
          ...resolvedData?.data,
          date: new Date(resolvedData.data.date).toISOString().split('T')[0],
          amount: resolvedData.data.amount,
          categoryId: resolvedData.data.category.id,
          paymentMethodId: resolvedData.data.paymentMethod.id
        }
      });
    }
    else {
      const today = new Date().toISOString().split('T')[0];
      this.form.patchValue({ date: today });
    }

    this.loadCategories();
    this.loadPaymentMethods();
  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      date: ['', [Validators.required]],
      amount: ['', [Validators.required]],
      description: ['', []],
      categoryId: ['', [Validators.required]],
      paymentMethodId: ['', [Validators.required]]
    });
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (response) => {
        if (response && response.data) {
          this.categories = response.data.items.sort((a: Category, b: Category) =>
            a.name.localeCompare(b.name)
          );
        } else {
          this.categories = [];
        }
      },
      error: (err) => {
        console.error('Erro ao carregar as categorias', err);
      }
    });
  }

  loadPaymentMethods(): void {
    this.paymentMethodService.getAll().subscribe({
      next: (response) => {
        if (response && response.data) {
          this.paymentMethods = response.data.items.sort((a: PaymentMethod, b: PaymentMethod) =>
            a.name.localeCompare(b.name)
          );
        } else {
          this.paymentMethods = [];
        }
      },
      error: (err) => {
        console.error('Erro ao carregar as formas de pagamento', err);
      }
    });
  }

  saveTransaction(): void {
    if (this.form.dirty && this.form.valid) {
      this.changesSaved = true;

      const formValues = this.form.value;

      const transactionData = {
        id: formValues.id || null,
        date: formValues.date,
        amount: formValues.amount,
        description: formValues.description,
        category: { id: formValues.categoryId },
        paymentMethod: { id: formValues.paymentMethodId }
      } as Transaction;

      this.transactionService.save(transactionData).subscribe({
        next: () => {
          const msg = this.isEditMode
            ? 'Lançamento atualizado com sucesso!'
            : 'Lançamento cadastrado com sucesso!';
          this.processSuccess(msg, '/transaction/list');
        },
        error: (error) => this.processFail(error)
      });
    }
  }
}
