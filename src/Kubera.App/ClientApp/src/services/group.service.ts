import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Group } from 'src/models/group.model';
import { GroupTotal } from 'src/models/groupTotal.model';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  constructor(private readonly httpClient: HttpClient,
              private readonly settingsService: SettingsService) {

  }

  public getAll(): Observable<Group[]> {
    const url = this.settingsService.endpoints.get.group;

    return this.httpClient.get<Group[]>(url);
  }

  public getTotals(currencyId: string): Observable<GroupTotal[]> {
    const url = `${this.settingsService.endpoints.get.groupTotal}?currencyId=${currencyId}`;

    return this.httpClient.get<GroupTotal[]>(url);
  }
}
