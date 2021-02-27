import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Asset } from 'src/models/asset.model';
import { AssetTotal } from 'src/models/assetTotal.model';
import { Filter, Order } from 'src/models/filtering.model';
import { HttpUtilsService } from './http-utils.service';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class AssetService {
  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService,
              private readonly httpUtilsService: HttpUtilsService) {

  }

  public getAll(): Observable<Asset[]> {
    const url = this.settingsService.endpoints.get.assets;

    return this.httpClient.get<Asset[]>(url);
  }

  public getTotals(currencyId: string, order?: Order, filter?: Filter): Observable<AssetTotal[]> {
    const url = this.httpUtilsService.createUrl(this.settingsService.endpoints.get.assetsTotal, null, order, filter, `currencyId=${currencyId}`);

    return this.httpClient.get<AssetTotal[]>(url);
  }
}
