import { Component, OnInit } from '@angular/core';
import { UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';
import { DataService } from 'src/app/data.service';
import { adminLogin } from 'src/app/data/data';

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

  constructor(private dataService: DataService) { }

  submitLogin(): void {
    if (this.loginForm.value.userId != '' && this.loginForm.value.password != '')
    {
      var loginSubmit: adminLogin = {
        userName: this.loginForm.value.userId,
        password: this.loginForm.value.password
      }

      this.dataService.sendAdminLogin(loginSubmit).subscribe(al =>
        {
          localStorage.setItem("jwt", al.bearerToken);
          localStorage.setItem("teamNum", "0");
          localStorage.setItem("realName", "");
        })

    }
  }

}
