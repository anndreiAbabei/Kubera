import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { AssetTotal } from 'src/models/assetTotal.model';
import { AssetService } from 'src/services/asset.service';
import { ErrorHandlerService } from 'src/services/errorHandler.service';

@Component({
    selector: 'app-dashboard-assets',
    templateUrl: './dashboard-assets.component.html',
    styleUrls: ['./dashboard-assets.component.scss']
})
/** dashboard-assets component*/
export class DashboardAssetsComponent implements AfterViewInit {
    public resultsLength = 0;
    public isLoadingResults = false;
    public noResult = false;
    public displayedColumns: string[] = ['name', 'total', 'actual', 'increase'];
    public canLoadMore = true;
    public assets: AssetTotal[];
    public readonly itemsPerPage = 30;
  
    @ViewChild(MatSort) sort: MatSort;
  
    constructor(private readonly assetService: AssetService,
        private readonly errorHandlering: ErrorHandlerService) {  }
  
    public ngAfterViewInit(): void {
      this.sort.sortChange
        .subscribe(() => {
            this.assets = this.sortAssets(this.assets);
        });
  
      this.refreshAssets();
    }
  
    public refreshAssets(): void {
        this.isLoadingResults = true;

        this.assetService.getTotals()
            .subscribe(a => this.assets = this.sortAssets(a),
                       e => this.errorHandlering.handle(e),
                       () => this.isLoadingResults = false);
    }

    private sortAssets(assets: AssetTotal[]): AssetTotal[] {
        const direction = this.sort.active && this.sort.direction === 'desc'
                ? -1
                : 1;
                
        return assets.sort((a1, a2) => a1.name.localeCompare(a2.name) * direction);
    }
  }
  