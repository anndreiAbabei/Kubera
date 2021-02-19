import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RouterModule } from '@angular/router';
import { applicationPaths as ApplicationPaths } from './api-authorization.constants';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    RouterModule.forChild(
      [
        { path: ApplicationPaths.register, component: LoginComponent },
        { path: ApplicationPaths.profile, component: LoginComponent },
        { path: ApplicationPaths.login, component: LoginComponent },
        { path: ApplicationPaths.loginFailed, component: LoginComponent },
        { path: ApplicationPaths.loginCallback, component: LoginComponent },
        { path: ApplicationPaths.logOut, component: LogoutComponent },
        { path: ApplicationPaths.loggedOut, component: LogoutComponent },
        { path: ApplicationPaths.logOutCallback, component: LogoutComponent }
      ]
    )
  ],
  declarations: [LoginMenuComponent, LoginComponent, LogoutComponent],
  exports: [LoginMenuComponent, LoginComponent, LogoutComponent]
})
export class ApiAuthorizationModule { }
