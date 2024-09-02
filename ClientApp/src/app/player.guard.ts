import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class PlayerGuard implements CanActivate {

  constructor(private jwtHelper: JwtHelperService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // get the jwt token which are present in the local storage

    const token = localStorage.getItem("jwt");
    const expectedRole = 1;

    // Check if the token is expired or not and if token is expired then redirect to login page and return false
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }

  checkRole(expectedRole: string): boolean {
    const token = localStorage.getItem("jwt");
    if (!token)
    {
      return false;
    }

    var decodedToken = this.jwtHelper.decodeToken(token);
    console.log(decodedToken);

    if (!decodedToken)
    {
      return false;
    }

    if (decodedToken["role"] && decodedToken["role"]?.includes(expectedRole)) {
      return true;
    }

    return false;
  }

}
