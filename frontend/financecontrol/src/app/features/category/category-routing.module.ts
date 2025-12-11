import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CategoryComponent } from './category.component';
import { CategoryFormComponent } from './components/category-form/category-form.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';
import { CategoryService } from 'src/app/shared/services/category.service';

const routes: Routes = [{
  path: '', component: CategoryComponent,
  children: [
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: 'list', component: CategoryListComponent,  canActivate: [authGuard]  }, 
    { path: 'new', component: CategoryFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
    { path: 'edit/:id', component: CategoryFormComponent, 
      canActivate: [authGuard],  
      canDeactivate: [canDeactivateForm], resolve: {
      category: genericResolver(CategoryService, (service, id) => service.getById(id))
    }},
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CategoryRoutingModule { }
