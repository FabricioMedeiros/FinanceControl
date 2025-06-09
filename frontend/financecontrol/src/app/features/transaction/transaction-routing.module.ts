import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionComponent } from './transaction.component';
import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';
import { TransactionService } from 'src/app/shared/services/transaction.service';
import { TransactionFormComponent } from './components/transaction-form/transaction-form.component';
import { TransactionListComponent } from './components/transaction-list/transaction-list.component';

const routes: Routes = [{
  path: '', component: TransactionComponent,
  children: [
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: 'list', component: TransactionListComponent,  canActivate: [authGuard]  }, 
    { path: 'new', component: TransactionFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
    { path: 'edit/:id', component: TransactionFormComponent, 
      canActivate: [authGuard],  
      canDeactivate: [canDeactivateForm], resolve: {
      transaction: genericResolver(TransactionService, (service, id) => service.getTransactionById(id))
    }},
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransactionRoutingModule { }
