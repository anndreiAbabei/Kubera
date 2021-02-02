import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../../services/settings.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  public appName: string;

  constructor(private readonly settingService: SettingsService) {

  }

  ngOnInit(): void {
    this.appName = this.settingService.fullAppName;
  }
}
