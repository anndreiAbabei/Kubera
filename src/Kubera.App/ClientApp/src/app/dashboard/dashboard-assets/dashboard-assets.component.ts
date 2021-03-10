import { AfterViewInit, Component, Input, OnChanges, OnDestroy, SimpleChanges, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { AssetTotal, AssetTotals } from 'src/models/assetTotal.model';
import { Currency } from 'src/models/currency.model';
import { Filter, Order } from 'src/models/filtering.model';
import { AssetService } from 'src/services/asset.service';
import { CurrencyService } from 'src/services/currency.service';
import { ErrorHandlerService } from 'src/services/errorHandler.service';
import { EventService } from 'src/services/event.service';
import { UserService } from 'src/services/user.service';

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
    public total = AssetTotals.Empty;
    public selectedCurrency: Currency;
    private currencies: Currency[];
    public readonly itemsPerPage = 30;

    @Input()
    public filter: Filter;

    @ViewChild(MatSort) sort: MatSort;

    constructor(private readonly assetService: AssetService,
        private readonly currencyService: CurrencyService,
        private readonly errorHandlering: ErrorHandlerService,
        private readonly eventService: EventService,
        private readonly userService: UserService) {  }

    public async ngAfterViewInit(): Promise<void> {
      this.sort.sortChange.subscribe(() => this.total.assets = this.sortAssets(this.total.assets));

      await this.refreshAssets();
      this.eventService.updateTransaction.subscribe(async () => await this.refreshAssets());
      this.eventService.refreshRequested.subscribe(async () => await this.refreshAssets());
      this.eventService.selectedCurrencyChanged.subscribe(async c => await this.refreshAssets(c.id));
    }

    public ngOnDestroy(): void {
      this.eventService.updateTransaction.unsubscribe();
      this.eventService.refreshRequested.unsubscribe();
      this.eventService.selectedCurrencyChanged.unsubscribe();
    }

    public async ngOnChanges(changes: SimpleChanges): Promise<void> {
      if (!changes['filter'].firstChange) {
        await this.refreshAssets();
      }
    }

    public async refreshAssets(currencyId?: string): Promise<void> {
      try {
        this.setIsLoading(true);

        let selectedCurrencyId = currencyId;

        if (!selectedCurrencyId) {
          const user = await this.userService.getUserInfo().toPromise();
          this.noResult = !user?.settings?.prefferedCurrency;

          if (this.noResult) {
            this.currencies = await this.currencyService.getAll().toPromise();
            this.noResult = this.currencies.length <= 0;
            selectedCurrencyId = this.currencies[0].id;
          } else {
            selectedCurrencyId = user.settings.prefferedCurrency;
          }
        } else {
          this.noResult = false;
        }

        if (!this.noResult) {
          const order = this.sort.active && this.sort.direction === 'asc'
                          ? Order.ascending
                          : Order.descending;

          this.total = await this.assetService.getTotals(selectedCurrencyId, order, this.filter).toPromise();

          this.noResult = this.total.assets.length <= 0;
        }

        if (!this.currencies) {
          this.currencies = await this.currencyService.getAll().toPromise();
        }
        this.selectedCurrency = this.currencies.find(c => c.id === selectedCurrencyId);
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
