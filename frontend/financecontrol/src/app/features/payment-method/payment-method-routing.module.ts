import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';
import { PaymentMethodService } from 'src/app/shared/services/payment-method.service';
import { PaymentMethodComponent } from './payment-method.component';
import { PaymentMethodFormComponent } from './components/payment-method-form/payment-method-form.component';
import { PaymentMethodListComponent } from './components/payment-method-list/payment-method-list.component';

const routes: Routes = [{
  path: '', component: PaymentMethodComponent,
  children: [
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: 'list', component: PaymentMethodListComponent,  canActivate: [authGuard]  }, 
    { path: 'new', component: PaymentMethodFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
    { path: 'edit/:id', component: PaymentMethodFormComponent, 
      canActivate: [authGuard],  
      canDeactivate: [canDeactivateForm], resolve: {
      paymentMethod: genericResolver(PaymentMethodService, (service, id) => service.getPaymentMethodById(id))
    }},
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PaymentMethodRoutingModule { }
