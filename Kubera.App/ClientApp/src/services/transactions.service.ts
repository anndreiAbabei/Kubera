import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Paging, DateFilter } from '../models/filtering.model';
import { Transaction, TransactionsResponse } from '../models/transactions.model';
import { HttpUtilsService } from './http-utils.service';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {
  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService,
              private readonly httpUtilsService: HttpUtilsService) {

  }

  public getAll(paging?: Paging, filter?: DateFilter): Observable<TransactionsResponse> {
    const url = this.httpUtilsService.createUrl(this.settingsService.endpoints.get.transactions, paging, filter);

    const page: number = paging ? paging.page : undefined;
    const items: number = paging ? paging.items : undefined;

    return this.httpClient.get<Transaction[]>(url, { observe: 'response' })
      .pipe(map(r => {

        return {
          currentPage: page,
          itemsPerPage: items,
          totalPages: this.httpUtilsService.getTotalPagesFromHeader(r.headers),
          transactions: r.body
        } as TransactionsResponse;
      }));
  }

  public delete(transactionId: string): Observable<Object> {
    return this.httpClient.delete(this.settingsService.endpoints.delete.transaction + '/' + transactionId);
  }
}
