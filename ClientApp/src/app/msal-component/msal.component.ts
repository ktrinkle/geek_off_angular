import { Component, OnInit } from '@angular/core';
import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-root',
  template: '',
})

// This component is used only to avoid Angular reload
// when doing acquireTokenSilent()

export class MsalComponent {
  constructor(private Msal: MsalService) {
  }
}
