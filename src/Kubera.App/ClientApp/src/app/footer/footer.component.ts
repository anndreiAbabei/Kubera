import { Component } from '@angular/core';
import { SettingsService } from 'src/services/settings.service';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  public appName: string;
  public version: string;

  constructor(private readonly settingService: SettingsService) {
    this.appName = this.settingService.fullAppName;
    this.version = this.settingService.appVersion;
  }
}
