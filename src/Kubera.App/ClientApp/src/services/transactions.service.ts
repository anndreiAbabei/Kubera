import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Paging, DateFilter, Order } from '../models/filtering.model';
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

  public getAll(paging?: Paging, ascending?: Order, filter?: DateFilter): Observable<TransactionsResponse> {
    const url = this.httpUtilsService.createUrl(this.settingsService.endpoints.get.transactions, paging, ascending, filter);

    const page: number = paging ? paging.page : undefined;
    const items: number = paging ? paging.items : undefined;

    return this.httpClient.get<Transaction[]>(url, { observe: 'response' })
      .pipe(map(r => {

        return {
          currentPage: page,
          itemsPerPage: items,
          totalItems: this.httpUtilsService.getTotalPagesFromHeader(r.headers),
          transactions: r.body
        } as TransactionsResponse;
      }));
  }

  public create(transaction: Transaction): Observable<Transaction> {
    return this.httpClient.post<Transaction>(this.settingsService.endpoints.post.transaction, transaction);
  }

  public update(transaction: Transaction): Observable<void> {
    return this.httpClient.put<void>(`${this.settingsService.endpoints.put.transaction}/${transaction.id}`, transaction);
  }

  public delete(transactionId: string): Observable<Object> {
    return this.httpClient.delete(`${this.settingsService.endpoints.delete.transaction}/${transactionId}`);
  }
}
