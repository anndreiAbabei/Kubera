import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Asset } from 'src/models/asset.model';
import { AssetTotal } from 'src/models/assetTotal.model';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class AssetService {
  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService) {

  }

  public getAll(): Observable<Asset[]> {
    const url = this.settingsService.endpoints.get.assets;

    return this.httpClient.get<Asset[]>(url);
  }

  public getTotals(currencyId: string): Observable<AssetTotal[]> {
    const url = `${this.settingsService.endpoints.get.assetsTotal}?currencyId=${currencyId}`;

    return this.httpClient.get<AssetTotal[]>(url);
  }
}
