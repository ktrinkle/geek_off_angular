import { Component, OnInit } from '@angular/core';
import { UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DataService } from 'src/app/data.service';
import { adminLogin } from 'src/app/data/data';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent {

  public loginForm: UntypedFormGroup = new UntypedFormGroup({
    userId: new UntypedFormControl('', [Validators.pattern('[0-9]*')]),
    password: new UntypedFormControl()
  });

  constructor(private router: Router, private authService: AuthService) { }

  submitLogin(): void {
    if (this.loginForm.value.userId != '' && this.loginForm.value.password != '')
    {
      var loginSubmit: adminLogin = {
        userLogin: {
          userName: this.loginForm.value.userId,
          password: this.loginForm.value.password
        }
      }

      this.authService.processLoginAdmin(loginSubmit);
      this.router.navigate(["/home"]);
    }
  }

}
