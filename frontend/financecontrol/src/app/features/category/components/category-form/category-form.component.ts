import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { CategoryService } from 'src/app/shared/services/category.service';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';
import { Category } from '../../models/category';

@Component({
  selector: 'app-category-form',
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.css']
})
export class CategoryFormComponent extends BaseFormComponent<Category> implements OnInit {
  categoryTypes = [
    { label: 'Receita', value: 0 },
    { label: 'Despesa', value: 1 }
  ];
  
  constructor(
    fb: FormBuilder,
    router: Router,
    route: ActivatedRoute,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    private categoryService: CategoryService
  ) {
    super(fb, router, route, toastr, spinner);

    this.validationMessages = {
      name: { required: 'Informe o nome' },
      description: { required: 'Informe a descrição' },
      type: { required: 'Informe o tipo' }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();

    const resolvedData = this.route.snapshot.data['category'];

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
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      type: ['', [Validators.required]],
    });
  }
  
  saveCategory() {
    if (this.form.dirty && this.form.valid) {
      this.changesSaved = true;    

      const categoryData = Object.fromEntries(
        Object.entries(this.form.value).filter(([key, value]) => key !== 'id' || (value && value !== ''))
      ) as unknown as Category;    

      if (this.isEditMode) {
        this.categoryService.updateCategory(categoryData).subscribe({
          next: () => this.processSuccess('Categoria atualizada com sucesso!', '/category/list'),
          error: (error) =>  this.processFail(error)
        });
      } else {
        this.categoryService.registerCategory(categoryData).subscribe({
          next: () => this.processSuccess('Categoria cadastrada com sucesso!', '/category/list'),
          error: (error) => {
            this.processFail(error);
          }
        });
      }
    }
  }
}
