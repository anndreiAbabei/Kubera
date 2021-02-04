import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { HomeComponent } from './home/home.component';
import { DashboardAssetsComponent } from './dashboard/dashboard-assets/dashboard-assets.component';
import { DashboardFilterComponent } from './dashboard/dashboard-filter/dashboard-filter.component';
import { DashboardGroupsComponent } from './dashboard/dashboard-groups/dashboard-groups.component';
import { DashboardTransactionsComponent } from './dashboard/dashboard-transactions/dashboard-transactions.component';
import { LoadingComponent } from './shared/loading/loading.component';
import { DashboardCreateTransactionComponent } from './dashboard/dashboard-create-transaction/dashboard-create-transaction.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    DashboardComponent,
    HomeComponent,
    DashboardAssetsComponent,
    DashboardFilterComponent,
    DashboardGroupsComponent,
    DashboardTransactionsComponent,
    DashboardCreateTransactionComponent,
    LoadingComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    NgbModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
    { path: '', component: HomeComponent },
    { path: 'dashboard', component: DashboardComponent, pathMatch: 'full', canActivate: [AuthorizeGuard] },
], { relativeLinkResolution: 'legacy' }),
    BrowserAnimationsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
