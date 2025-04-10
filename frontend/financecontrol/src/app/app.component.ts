import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

import { LocalStorageUtils } from './core/utils/localstorage';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'financecontrol';
  showOnlyComponentRoute: boolean = false;
  token: string | null = "";

  localStorageUtils = new LocalStorageUtils();

  constructor(private router: Router) {
    this.router.events.pipe(
      filter((event): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.updateShowOnlyComponentRoute(event.urlAfterRedirects);
    });
  }

  private updateShowOnlyComponentRoute(url: string): void {
    const specialRoutes = ['/account/login', '/account/register'];
    this.showOnlyComponentRoute = specialRoutes.some(route => url.includes(route));
  }

  isUserLoggedIn(): boolean {
    this.token = this.localStorageUtils.getTokenUser();
    return this.token !== null;
  }
}
