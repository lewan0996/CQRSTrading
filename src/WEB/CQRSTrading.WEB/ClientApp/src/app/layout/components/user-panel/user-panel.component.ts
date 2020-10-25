import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { Subscription } from 'rxjs';
import { b2cPolicies } from 'src/app/auth/msal-config';


@Component({
  selector: 'app-user-panel',
  templateUrl: 'user-panel.component.html',
  styleUrls: ['./user-panel.component.scss']
})

export class UserPanelComponent implements OnInit, OnDestroy {
  @Input()
  menuMode: string;

  isLoggedIn = false;
  userName: string;

  loginSuccessSubscription: Subscription;
  loginFailureSubscription: Subscription;

  notLoggedInMenuItems = [{
    text: 'Log in',
    icon: 'user',
    onClick: () => {
      this.login();
    }
  }];

  loggedInMenuItems = [
    {
      text: 'My account',
      icon: 'user'
    },
    {
      text: 'Log out',
      icon: 'user',
      onClick: () => {
        this.logout();
      }
    }
  ];

  constructor(private authService: MsalService, private broadcastService: BroadcastService) { }

  ngOnInit(): void {
    this.checkAccount();

    this.loginSuccessSubscription = this.broadcastService.subscribe('msal:loginSuccess', (success) => {

      // We need to reject id tokens that were not issued with the default sign-in policy.
      // "acr" claim in the token tells us what policy is used (NOTE: for new policies (v2.0), use "tfp" instead of "acr")
      // To learn more about b2c tokens, visit https://docs.microsoft.com/en-us/azure/active-directory-b2c/tokens-overview
      if (success.idToken.claims.acr === b2cPolicies.names.resetPassword) {
        window.alert('Password has been reset successfully. \nPlease sign-in with your new password');
        return this.authService.logout();
      }

      console.log('login succeeded. id token acquired at: ' + new Date().toString());
      console.log(success);

      this.checkAccount();
    });

    this.loginFailureSubscription = this.broadcastService.subscribe('msal:loginFailure', (error) => {
      console.log('login failed');
      console.log(error);

      if (error.errorMessage) {
        // Check for forgot password error
        // Learn more about AAD error codes at https://docs.microsoft.com/en-us/azure/active-directory/develop/reference-aadsts-error-codes
        if (error.errorMessage.indexOf('AADB2C90118') > -1) {
          this.authService.loginRedirect(b2cPolicies.authorities.resetPassword);
        }
      }
    });

    // redirect callback for redirect flow (IE)
    this.authService.handleRedirectCallback((authError, response) => {
      if (authError) {
        console.error('Redirect Error: ', authError.errorMessage);
        return;
      }

      console.log('Redirect Success: ', response);
    });
  }

  ngOnDestroy(): void {
    this.loginSuccessSubscription.unsubscribe();
    this.loginFailureSubscription.unsubscribe();
  }

  checkAccount(): void {
    const account = this.authService.getAccount();
    this.isLoggedIn = !!account;

    this.userName = this.isLoggedIn ? account.name : 'My account';
  }

  login(): void {
    this.authService.loginRedirect();
  }

  logout(): void {
    this.authService.logout();
  }
}
