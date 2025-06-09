
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { format } from 'date-fns';

import { BaseListComponent } from 'src/app/shared/components/base-list/base-list.component';
import { TransactionService } from 'src/app/shared/services/transaction.service';
import { Transaction } from '../../models/transaction';
import { Category } from '../../models/category';
import { PaymentMethod } from '../../models/payment-method';
import { CategoryService } from 'src/app/shared/services/category.service';
import { PaymentMethodService } from 'src/app/shared/services/payment-method.service';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent extends BaseListComponent<Transaction> implements OnInit {
  categories: Category[] = [];
  paymentMethods: PaymentMethod[] = [];

  pageSizeOptions: number[] = [10, 25, 50, 100];
  selectedPageSize = 50;
  selectedCategory: string = 'Todas';
  selectedPaymentMethod: string = 'Todos';
  startDate: Date = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
  endDate: Date = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0);

  constructor(private transactionService: TransactionService,
    private categoryService: CategoryService,
    private paymentMethodService: PaymentMethodService,
    router: Router,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    modalService: BsModalService) {
    super(transactionService, router, toastr, spinner, modalService);

    const today = new Date();
    this.startDate = new Date(today.getFullYear(), today.getMonth(), 1);
    this.endDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
  }

  override ngOnInit(): void {
    this.loadCategories();
    this.loadPaymentMethods();

    const storedPage = this.service.getLocalCurrentPageList();

    if (storedPage) this.currentPage = parseInt(storedPage, 10);

    const filters = this.transactionService.getLocalSearchTerm();

    if (filters) {
      this.pageSize = filters.pageSize ?? this.pageSize;
      this.selectedCategory = filters.category ?? this.selectedCategory;
      this.selectedPaymentMethod = filters.paymentMethod ?? this.selectedPaymentMethod;
      this.startDate = filters.startDate ?? this.startDate;
      this.endDate = filters.endDate ?? this.endDate;
    }

    this.loadItems();
  }

  override loadItems(): void {
    this.spinner.show();
    this.loadingData = true;
    this.items = [];

    try {
      if (!this.startDate || !this.endDate) {
        this.spinner.hide();
        return;
      }

      const formattedStart = format(this.startDate, 'MM/dd/yyyy');
      const formattedEnd = format(this.endDate, 'MM/dd/yyyy');

      const categoryId = this.selectedCategory !== "Todas" ? this.selectedCategory.trim() : undefined;
      const paymentMethodId = this.selectedPaymentMethod !== "Todos" ? this.selectedPaymentMethod.trim() : undefined;

      this.transactionService.getTransactionsByPeriod(
        formattedStart,
        formattedEnd,
        categoryId,
        paymentMethodId,
        this.currentPage,
        this.pageSize
      ).subscribe({
        next: response => {
          this.processLoadSuccess(response);
        },
        error: error => {
          this.processLoadFail(error);
        },
        complete: () => {
          this.processCompleted();
        }
      });
    } catch (error) {
      console.error("Erro ao formatar ou processar os filtros:", error);
      this.spinner.hide();
      this.loadingData = false;
    }
  }

  override processLoadSuccess(response: any) {
    this.items = response.data.items
      .map((item: any, index: number): { date: string; _i: number;[key: string]: any } => ({
        ...item,
        _i: index
      }))
      .sort((a: { date: string; _i: number }, b: { date: string; _i: number }) =>
        new Date(b.date).getTime() - new Date(a.date).getTime() || b._i - a._i
      )
      .map(({ _i, ...item }: { _i: number;[key: string]: any }) => item);

    this.currentPage = response.data.page;
    this.totalPages = Math.ceil(response.data.totalRecords / response.data.pageSize);
  }

  override onSearch(): void {
    this.currentPage = 1;
    this.transactionService.saveLocalSearchTerm(this.pageSize,
      this.selectedCategory,
      this.selectedPaymentMethod,
      this.startDate,
      this.endDate);

    this.loadItems();
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (response) => {
        if (response && response.data) {
          this.categories = response.data.items;
        } else {
          this.categories = [];
        }
      },
      error: (err) => {
        this.errorMessage = 'Erro ao carregar as catgorias';
        console.error('Erro ao carregar as categorias', err);
      }
    });
  }

  loadPaymentMethods(): void {
    this.paymentMethodService.getAll().subscribe({
      next: (response) => {
        if (response && response.data) {
          this.paymentMethods = response.data.items;
        } else {
          this.paymentMethods = [];
        }
      },
      error: (err) => {
        this.errorMessage = 'Erro ao carregar as formas de pagamento';
        console.error('Erro ao carregar as formas de pagamento', err);
      }
    });
  }

  onPageSizeChange(): void {
    this.pageSize = this.selectedPageSize;
    this.loadItems();
  }

  onCategoryChange(): void {
    this.loadItems();
  }

  onPaymentMethodChange() {
    this.loadItems();
  }

  isCurrentRoute(url: string): boolean {
    return url.includes('/transaction');
  }
}
