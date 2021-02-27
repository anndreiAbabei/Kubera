import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Currency } from 'src/models/currency.model';
import { CachedService } from './cached.service';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService extends CachedService {

  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService) {
    super();
  }

  public getAll(): Observable<Currency[]> {
    const url = this.settingsService.endpoints.get.currencies;

    return super.getOrAddInCache(url, () => this.httpClient.get<Currency[]>(url), super.persistentCache);
  }
}
