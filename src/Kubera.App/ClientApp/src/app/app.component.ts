import { Component } from '@angular/core';
import { ThemeService } from 'src/services/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'app';

  constructor(private readonly themeService: ThemeService) {
    this.themeService.load();
  }
}
