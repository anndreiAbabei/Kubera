import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Filter, Order } from 'src/models/filtering.model';
import { Group } from 'src/models/group.model';
import { GroupTotal } from 'src/models/groupTotal.model';
import { HttpUtilsService } from './http-utils.service';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService,
              private readonly httpUtilsService: HttpUtilsService) {

  }

  public getAll(): Observable<Group[]> {
    const url = this.settingsService.endpoints.get.group;

    return this.httpClient.get<Group[]>(url);
  }

  public getTotals(currencyId: string, order?: Order, filter?: Filter): Observable<GroupTotal[]> {
    const url = this.httpUtilsService.createUrl(this.settingsService.endpoints.get.groupTotal, null, order, filter, `currencyId=${currencyId}`);

    return this.httpClient.get<GroupTotal[]>(url);
  }
}
