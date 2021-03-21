import { Component } from '@angular/core';
import { Filter } from 'src/models/filtering.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  public filter: Filter;

  public filterChanged(filter: Filter): void {
    this.filter = filter;
  }
}
