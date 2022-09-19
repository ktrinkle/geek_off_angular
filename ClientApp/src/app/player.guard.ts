import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class PlayerGuard implements CanActivate {

  constructor(private jwtHelper: JwtHelperService, private router: Router) {
  }
  canActivate() {

    //get the jwt token which are present in the local storage
    const token = localStorage.getItem("jwt");

    //Check if the token is expired or not and if token is expired then redirect to login page and return false
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }

    this.router.navigate(["home"]);
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
