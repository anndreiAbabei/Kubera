import { Component, OnInit } from '@angular/core';
import { Filter } from 'src/models/filtering.model';
import { SettingsService } from '../../services/settings.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  public appName: string;
  public version: string;

  public filter: Filter;

  constructor(private readonly settingService: SettingsService) {

  }

  public ngOnInit(): void {
    this.appName = this.settingService.fullAppName;
    this.version = this.settingService.appVersion;
  }

  public filterChanged(filter: Filter): void {
    this.filter = filter;
  }
}
