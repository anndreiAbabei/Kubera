import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DateFilter, Paging } from '../models/filtering.model';

@Injectable({
  providedIn: 'root'
})
export class HttpUtilsService {
  public headerNumberOfPagesName = "X-PageCount";

  public createUrl(baseUrl: string, paging?: Paging, filter?: DateFilter): string {
    let url = baseUrl;

    if (paging || filter)
      url += '?';

    if (paging)
      url += `page=${paging.page}&items=${paging.items}`;

    if (paging ?? filter)
      url += '&';

    if (filter) {
      if (filter.from)
        url += `from=${filter.from}`;

      if (filter.from && filter.to)
        url += '&';

      if (filter.to)
        url += `to=${filter.to}`;
    }

    return url;
  }

  public getTotalPagesFromHeader(headers: HttpHeaders): number {
    return parseInt(headers.get(this.headerNumberOfPagesName));
  }
}
