import { Injectable } from '@angular/core';
import { CanActivate, UrlTree, Router, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
    providedIn: 'root'
})

export class AdminAuthGuard implements CanActivate{

    constructor(private router: Router, private authService: AuthService) {

    }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean{
        if(!(this.authService.isUserAdmin())){
            this.router.navigate([this.router.url]);
            return false;
        }

        return true;
    }
}