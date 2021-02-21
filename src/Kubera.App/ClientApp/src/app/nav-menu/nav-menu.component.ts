import { Component, OnInit } from '@angular/core';
import { ThemeService } from 'src/services/theme.service';
import { SettingsService } from '../../services/settings.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {
  public isExpanded = false;
  public appName: string;
  public dark: boolean;

  constructor(private readonly settingService: SettingsService,
    private readonly themeService: ThemeService) {

  }

  public ngOnInit(): void {
    this.appName = this.settingService.fullAppName;
    this.dark = this.themeService.currentActive() === this.themeService.dark;
  }

  public collapse(): void {
    this.isExpanded = false;
  }

  public toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

  public toggleDarkMode(): void {
    this.dark = !this.dark;

    this.themeService.update(this.dark ? this.themeService.dark : this.themeService.light);
  }
}
