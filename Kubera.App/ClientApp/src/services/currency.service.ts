import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Currency } from 'src/models/currency.model';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService {
  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService) {

  }

  public getAll(): Observable<Currency[]> {
    const url = this.settingsService.endpoints.get.currencies;

    return this.httpClient.get<Currency[]>(url);
  }
}
