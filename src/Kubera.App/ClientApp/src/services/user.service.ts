import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserInfo } from 'src/models/user-info.model';
import { CachedService } from './cached.service';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends CachedService {
  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService) {
    super();
  }

  public getUserInfo(): Observable<UserInfo> {
    const url = this.settingsService.endpoints.get.user;

    return super.getOrAddInCache(url, () => this.httpClient.get<UserInfo>(url));
  }

  public updateUserPrefferedCurrency(currencyId: any): Observable<Object> {
    const url = this.settingsService.endpoints.patch.userCurrency;
    const body = {
      currencyId: currencyId
    };
    const res = this.httpClient.patch(url, body);

    res.subscribe(() => super.remove(this.settingsService.endpoints.get.user));

    return res;
  }
}
