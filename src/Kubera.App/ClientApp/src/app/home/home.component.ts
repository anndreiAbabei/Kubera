import { Component } from '@angular/core';
import { SettingsService } from 'src/services/settings.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
/** home component*/
export class HomeComponent {
  public appName: string;

  constructor(settingService: SettingsService) {
    this.appName = settingService.appName;
  }
}
