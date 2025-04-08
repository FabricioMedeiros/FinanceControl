import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './features/navigation/components/home/home.component';
import { NotFoundComponent } from './features/navigation/components/not-found/not-found.component';

const routes: Routes = [ 
  { path: 'home', component: HomeComponent,  },
  { path: 'not-found', component: NotFoundComponent,  },
  { path: '**', component: NotFoundComponent }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
