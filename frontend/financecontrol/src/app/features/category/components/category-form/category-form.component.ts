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
      type: { required: 'Informe o tipo' }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();

    const resolvedData = this.route.snapshot.data['category'];

    if (resolvedData) {
      this.initializeForm({
        data: {
          ...resolvedData?.data,
        }
      });
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      name: ['', [Validators.required]],
      description: ['', []],
      type: ['', [Validators.required]],
    });
  }

  saveCategory() {
    if (this.form.dirty && this.form.valid) {
      this.changesSaved = true;
      this.entity = Object.assign({}, this.form.value);

      const request = this.categoryService.save(this.entity);

      request.subscribe({
        next: () => {
          const msg = this.isEditMode
            ? 'Categoria atualizada com sucesso!'
            : 'Categoria cadastrada com sucesso!';
          this.processSuccess(msg, '/category/list');
        },
        error: err => this.processFail(err)
      });
    }
  }
}
