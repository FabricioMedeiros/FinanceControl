import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './features/navigation/components/home/home.component';
import { NotFoundComponent } from './features/navigation/components/not-found/not-found.component';
import { ServiceUnavailableComponent } from './features/navigation/components/service-unavailable/service-unavailable.component';

const routes: Routes = [
  { path: '', redirectTo: 'account/login', pathMatch: 'full' },
  { path: 'home', component: HomeComponent},
  { path: 'account', loadChildren: () => import('./features/account/account.module').then(m => m.AccountModule) },
  { path: 'service-unavailable', component: ServiceUnavailableComponent},
  { path: 'not-found', component: NotFoundComponent, },
  { path: '**', component: NotFoundComponent }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
