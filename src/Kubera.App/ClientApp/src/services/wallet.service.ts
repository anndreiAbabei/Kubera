import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetAllWalletsResponse } from 'src/models/wallet.model';
import { HttpUtilsService } from './http-utils.service';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class WalletService {
  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService,
              private readonly httpUtilsService: HttpUtilsService) {

  }

  public getAll(): Observable<GetAllWalletsResponse> {
    const url = this.httpUtilsService.createUrl(this.settingsService.endpoints.get.wallet);

    return this.httpClient.get<GetAllWalletsResponse>(url);
  }
}
