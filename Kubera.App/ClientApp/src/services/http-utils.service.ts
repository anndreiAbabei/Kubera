import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DateFilter, Order, Paging } from '../models/filtering.model';

@Injectable({
  providedIn: 'root'
})
export class HttpUtilsService {
  public headerNumberOfPagesName = 'X-PageCount';

  public createUrl(baseUrl: string, paging?: Paging, order?: Order, filter?: DateFilter): string {
    let url = baseUrl;

    if (paging || order || filter) {
      url += '?';
    }

    if (paging) {
      url += `page=${paging.page}&items=${paging.items}`;

      if (order || filter) {
        url += '&';
      }
    }

    if (order) {
      url += `order=${order}`;
    }

    if (filter) {
      if ((paging || order) && (filter.from || filter.to)) {
        url += '&';
      }

      if (filter.from) {
        url += `from=${filter.from}`;
      }

      if (filter.from && filter.to) {
        url += '&';
      }

      if (filter.to) {
        url += `to=${filter.to}`;
      }
    }



    return url;
  }

  public getTotalPagesFromHeader(headers: HttpHeaders): number {
    return Number(headers.get(this.headerNumberOfPagesName));
  }
}
