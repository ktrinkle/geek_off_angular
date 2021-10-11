import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { AccountInfo } from '@azure/msal-common';

interface Account extends AccountInfo {
  idTokenClaims?: {
    roles?: string[]
  }
}

@Injectable({
  providedIn: 'root'
})
export class PlayerGuard implements CanActivate {

  constructor(private authService: MsalService) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const expectedRole = route.data.expectedRole;
    let account: Account = this.authService.instance.getAllAccounts()[0];

    if (!account.idTokenClaims?.roles) {
      window.alert('I\'m sorry. Please log out and log in again.');
      return false;
    } else if (!account.idTokenClaims?.roles?.includes(expectedRole)) {
      window.alert('I\'m sorry. You do not have access to this area. If you are supposed to, please log out and log in again.');
      return false;
    }

    return true;
  }

  checkAdmin(expectedRole: string): boolean {
    let account: Account = this.authService.instance.getAllAccounts()[0];

    if (!account.idTokenClaims?.roles) {
      return false;
    } else if (!account.idTokenClaims?.roles?.includes(expectedRole)) {
      return false;
    }

    return true;
  }

}
