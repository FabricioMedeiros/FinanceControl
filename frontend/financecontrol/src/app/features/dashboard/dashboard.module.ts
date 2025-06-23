import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgxSpinnerModule } from 'ngx-spinner';
import { NgChartsModule } from 'ng2-charts';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { DashboardDataComponent } from './components/dashboard-data/dashboard-data.component';
import { DashboardService } from './services/dashboard.service';

@NgModule({
  declarations: [
    DashboardComponent,
    DashboardDataComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    NgChartsModule
  ],
  providers:[DashboardService]
})
export class DashboardModule { }
