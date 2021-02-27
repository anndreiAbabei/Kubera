import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Filter, Order, Paging } from '../models/filtering.model';

@Injectable({
  providedIn: 'root'
})
export class HttpUtilsService {
  public headerNumberOfPagesName = 'X-PageCount';

  public createUrl(baseUrl: string, paging?: Paging, order?: Order, filter?: Filter, ...others: string[]): string {
    let url = baseUrl;

    const queryStrings: string[] = [];

    if (paging) {
      queryStrings.push(`page=${paging.page}`);
      queryStrings.push(`items=${paging.items}`);
    }

    if (order) {
      queryStrings.push(`order=${order}`);
    }

    if (filter) {
      if (filter.from) {
        queryStrings.push(`from=${filter.from.toISOString()}`);
      }
      if (filter.to) {
        queryStrings.push(`to=${filter.to.toISOString()}`);
      }
      if (filter.assetId) {
        queryStrings.push(`assetId=${filter.assetId}`);
      }
      if (filter.groupId) {
        queryStrings.push(`groupId=${filter.groupId}`);
      }
    }

    if (others && others.length > 0) {
      for (const qs of others) {
        queryStrings.push(qs);
      }
    }

    if (queryStrings.length > 0) {
      url += `?${queryStrings.join('&')}`;
    }

    return url;
  }

  public getTotalPagesFromHeader(headers: HttpHeaders): number {
    return Number(headers.get(this.headerNumberOfPagesName));
  }
}
