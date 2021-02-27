import { AfterViewInit, Component, Input, OnChanges, OnDestroy, SimpleChanges, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { AssetTotal } from 'src/models/assetTotal.model';
import { Currency } from 'src/models/currency.model';
import { Filter, Order } from 'src/models/filtering.model';
import { AssetService } from 'src/services/asset.service';
import { CurrencyService } from 'src/services/currency.service';
import { ErrorHandlerService } from 'src/services/errorHandler.service';
import { EventService } from 'src/services/event.service';

@Component({
    selector: 'app-dashboard-assets',
    templateUrl: './dashboard-assets.component.html',
    styleUrls: ['./dashboard-assets.component.scss']
})
/** dashboard-assets component*/
export class DashboardAssetsComponent implements AfterViewInit, OnChanges, OnDestroy {
    public resultsLength = 0;
    public isLoadingResults = false;
    public noResult = false;
    public displayedColumns: string[] = ['name', 'amount', 'total', 'actual', 'increase'];
    public canLoadMore = true;
    public assets: AssetTotal[];
    public selectedCurrency: Currency;
    private currencies: Currency[];
    public readonly itemsPerPage = 30;

    @Input()
    public filter: Filter;

    @ViewChild(MatSort) sort: MatSort;

    constructor(private readonly assetService: AssetService,
        private readonly currencyService: CurrencyService,
        private readonly errorHandlering: ErrorHandlerService,
        private readonly eventService: EventService) {  }

    public async ngAfterViewInit(): Promise<void> {
      this.sort.sortChange.subscribe(() => this.assets = this.sortAssets(this.assets));

      await this.refreshAssets();
      this.eventService.updateTransaction.subscribe(async () => await this.refreshAssets());
    }

    public ngOnDestroy(): void {
      this.eventService.updateTransaction.unsubscribe();
    }

    public async ngOnChanges(changes: SimpleChanges): Promise<void> {
      console.log('Assets');
      console.log(changes);
      await this.refreshAssets();
    }

    public async refreshAssets(): Promise<void> {
      try {
        this.setIsLoading(true);

        this.currencies = await this.currencyService.getAll().toPromise();
        this.noResult = this.currencies.length <= 0;

        if (!this.noResult) {
          const order = this.sort.active && this.sort.direction === 'asc'
                          ? Order.ascending
                          : Order.descending;

          this.selectedCurrency = this.currencies[0];
          this.assets = await this.assetService.getTotals(this.selectedCurrency.id, order, this.filter).toPromise();

          this.noResult = this.assets.length <= 0;
        }
      } catch (ex) {
        this.errorHandlering.handle(ex);
        this.noResult = true;
      } finally {
        this.setIsLoading(false);
      }
    }

    private sortAssets(assets: AssetTotal[]): AssetTotal[] {
        const direction = this.sort.active && this.sort.direction === 'desc'
                ? -1
                : 1;

        return assets.sort((a1, a2) => a1.name.localeCompare(a2.name) * direction);
    }

    private setIsLoading(isLoading: boolean): void {
      setTimeout(() => this.isLoadingResults = isLoading);
    }
  }
