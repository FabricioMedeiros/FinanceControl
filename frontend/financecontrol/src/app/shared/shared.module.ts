import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BaseListComponent } from './components/base-list/base-list.component';
import { BaseFormComponent } from './components/base-form/base-form.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    BaseListComponent,
    BaseFormComponent,
    SearchBarComponent,
    PaginationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class SharedModule { }
