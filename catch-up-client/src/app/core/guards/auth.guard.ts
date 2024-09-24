import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/logic/auth/auth.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService,
              private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      const isUserLoggedIn = this.authService.isLoggedIn();
      const isAdmin = this.authService.isAdmin();
      const loggedInUserId = this.authService.getUserId();
      const userIdFromRoute = route.paramMap.get('id');

      /*if(!isUserLoggedIn) {
        if(route.routeConfig?.path === 'login' || route.routeConfig?.path === 'register') {
          return true;
        }
        if(route.routeConfig?.path === 'forgot-password' || route.routeConfig?.path === 'reset-password/:email/:reset-token'){
          return  true;
        }
      }

      if(isUserLoggedIn && isAdmin) {
        if(route.routeConfig?.path === 'users' || route.routeConfig?.path === 'users/:id')
          return true;
      }

      if(isUserLoggedIn && (loggedInUserId === userIdFromRoute || isAdmin)) {
        return true;
      }

      if(isUserLoggedIn)
        this.router.navigate([`/users/${loggedInUserId}`]);
      else
        this.router.navigate(['/login']);
      return false;*/

      if(!isUserLoggedIn) {
        if(this.isPublicRoute(route))
          return true;

        this.router.navigate(['/login']);
        return false;
      }

      else if(isUserLoggedIn) {
        if(isAdmin) {
          if(this.isAdminRoute(route))
            return true;
        }
  
        if(loggedInUserId === userIdFromRoute) {
          return true;
        }

        this.router.navigate([`/users/${loggedInUserId}`]);
        return false;
      }
      
      return false;
  }

  private isPublicRoute(route: ActivatedRouteSnapshot): boolean {
    const publicRoutes = ['login', 'register', 'forgot-password', 'reset-password/:email/:reset-token'];
    return publicRoutes.includes(route.routeConfig?.path || '');
  }

  private isAdminRoute(route: ActivatedRouteSnapshot): boolean {
    const adminRoutes = ['users', 'users/:id', 'users/:id/change-password'];
    return adminRoutes.includes(route.routeConfig?.path || '');
  }
}