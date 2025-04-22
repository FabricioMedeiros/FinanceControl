import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { BaseListComponent } from 'src/app/shared/components/base-list/base-list.component';
import { Category } from '../../models/category';
import { CategoryService } from 'src/app/shared/services/category.service';
import { CategoryType } from '../../models/category-type.enum';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent extends BaseListComponent<Category> implements OnInit {
  constructor(categoryService: CategoryService,
    router: Router,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    modalService: BsModalService) {
    super(categoryService, router, toastr, spinner, modalService);
  }

  override ngOnInit(): void {
    this.fieldSearch = 'name';
    this.placeholderSearch = 'Pesquise pelo nome';
    super.ngOnInit();
  }

  isCurrentRoute(url: string): boolean {
    return url.includes('/category');
  }

  getCategoryTypeDescription(categoryType: CategoryType): string {
    if (categoryType === undefined || categoryType === null) {
      return 'Desconhecido';
    }
    switch (categoryType) {
      case CategoryType.Income:
        return 'Receita';
      case CategoryType.Expense:
        return 'Despesa';
      default:
        return 'Desconhecido';
    }
  }
}