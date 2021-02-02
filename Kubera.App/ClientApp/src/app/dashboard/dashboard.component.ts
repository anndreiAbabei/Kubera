import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {

  public test: string;

  constructor(private readonly httpClient: HttpClient) {

  }

  ngOnInit(): void {
    this.httpClient.get("/api/v1/asset")
      .subscribe(a => this.test = JSON.stringify(a));
  }
}
