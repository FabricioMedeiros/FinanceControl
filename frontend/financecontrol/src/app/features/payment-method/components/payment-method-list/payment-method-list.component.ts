import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { BaseListComponent } from 'src/app/shared/components/base-list/base-list.component';
import { PaymentMethod } from '../../models/payment-method';
import { PaymentMethodService } from 'src/app/shared/services/payment-method.service';

@Component({
  selector: 'app-payment-method-list',
  templateUrl: './payment-method-list.component.html',
  styleUrls: ['./payment-method-list.component.css']
})
export class PaymentMethodListComponent  extends BaseListComponent<PaymentMethod> implements OnInit {
  constructor(paymentMethodService: PaymentMethodService,
    router: Router,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    modalService: BsModalService) {
    super(paymentMethodService, router, toastr, spinner, modalService);
  }

  override ngOnInit(): void {
    this.fieldSearch = 'name';
    this.placeholderSearch = 'Pesquise pelo nome';
    super.ngOnInit();
  }

  isCurrentRoute(url: string): boolean {
    return url.includes('/payment-method');
  }
}
