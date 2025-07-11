import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountComponent } from './account.component';

import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { canActivate, canDeactivate } from './services/account.guard';

const routes: Routes = [
  {
    path: '', component: AccountComponent,
    children: [
      { path: 'register', component: RegisterComponent, canActivate: [canActivate], canDeactivate: [canDeactivate] },
      { path: 'login', component: LoginComponent, canActivate: [canActivate] }
    ]
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
