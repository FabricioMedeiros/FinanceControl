import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedRoutingModule } from './shared-routing.module';
import { SharedComponent } from './shared.component';
import { BaseListComponent } from './components/base-list/base-list.component';
import { BaseFormComponent } from './components/base-form/base-form.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationComponent } from './components/pagination/pagination.component';


@NgModule({
  declarations: [
    SharedComponent,
    BaseListComponent,
    BaseFormComponent,
    SearchBarComponent,
    PaginationComponent
  ],
  imports: [
    CommonModule,
    SharedRoutingModule
  ]
})
export class SharedModule { }
