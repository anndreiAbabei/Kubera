import { Injectable } from '@angular/core';
import { Endpoints } from '../models/endpoints.model';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  public appName = 'Loot';
  public fullAppName = 'Loot - App';
  public appVersion = 'v.alpha.2.0.1';
  public endpoints = new Endpoints();
}
