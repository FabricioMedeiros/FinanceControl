import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NavigationRoutingModule } from './navigation-routing.module';
import { NavigationComponent } from './navigation.component';
import { HomeComponent } from './components/home/home.component';
import { FooterComponent } from './components/footer/footer.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HeaderComponent } from './components/header/header.component';
import { LogoComponent } from './components/logo/logo.component';
import { MenuLoginComponent } from './components/menu-login/menu-login.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ServiceUnavailableComponent } from './components/service-unavailable/service-unavailable.component';


@NgModule({
  declarations: [
    NavigationComponent,
    HomeComponent,
    FooterComponent,
    SidebarComponent,
    HeaderComponent,
    LogoComponent,
    MenuLoginComponent,
    NotFoundComponent,
    ServiceUnavailableComponent
  ],
  imports: [
    CommonModule,
    NavigationRoutingModule
  ],
  exports:[
    HomeComponent,
    FooterComponent,
    SidebarComponent,
    HeaderComponent,
    LogoComponent,
    MenuLoginComponent,
    NotFoundComponent,
    ServiceUnavailableComponent
  ]
})
export class NavigationModule { }
