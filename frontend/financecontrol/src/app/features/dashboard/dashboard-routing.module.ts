import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';

import { DashboardDataComponent } from './components/dashboard-data/dashboard-data.component';
import { authGuard } from 'src/app/core/guards/auth.guard';

const routes: Routes = [
  {
    path: '', 
    component: DashboardComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: DashboardDataComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
