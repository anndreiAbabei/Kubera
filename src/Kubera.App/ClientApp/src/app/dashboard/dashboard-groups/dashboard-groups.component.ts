import { AfterViewInit, Component, Input, OnChanges, OnDestroy, SimpleChanges, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { Currency } from 'src/models/currency.model';
import { Filter, Order } from 'src/models/filtering.model';
import { GroupTotal } from 'src/models/groupTotal.model';
import { CurrencyService } from 'src/services/currency.service';
import { ErrorHandlerService } from 'src/services/errorHandler.service';
import { EventService } from 'src/services/event.service';
import { GroupService } from 'src/services/group.service';

@Component({
    selector: 'app-dashboard-groups',
    templateUrl: './dashboard-groups.component.html',
    styleUrls: ['./dashboard-groups.component.scss']
})
/** dashboard-groups component*/
export class DashboardGroupsComponent  implements AfterViewInit, OnChanges, OnDestroy {
  public resultsLength = 0;
  public isLoadingResults = false;
  public noResult = false;
  public displayedColumns: string[] = ['name', 'amount', 'total', 'actual', 'increase'];
  public canLoadMore = true;
  public groups: GroupTotal[];
  public selectedCurrency: Currency;
  private currencies: Currency[];
  public readonly itemsPerPage = 30;

  @Input()
  public filter: Filter;

  @ViewChild(MatSort) sort: MatSort;

  constructor(private readonly groupService: GroupService,
      private readonly currencyService: CurrencyService,
      private readonly errorHandlering: ErrorHandlerService,
      private readonly eventService: EventService) {  }

  public async ngAfterViewInit(): Promise<void> {
    this.sort.sortChange.subscribe(() => this.groups = this.sortGroups(this.groups));

    await this.refreshGroups();
    this.eventService.updateTransaction.subscribe(async () => await this.refreshGroups());
  }

  public async ngOnChanges(changes: SimpleChanges): Promise<void> {
    console.log('Groups');
    console.log(changes);
    await this.refreshGroups();
  }

  public ngOnDestroy(): void {
    this.eventService.updateTransaction.unsubscribe();
  }

  public async refreshGroups(): Promise<void> {
    try {
      this.setIsLoading(true);

      this.currencies = await this.currencyService.getAll().toPromise();
      this.noResult = this.currencies.length <= 0;

      if (!this.noResult) {
        const order = this.sort.active && this.sort.direction === 'asc'
                        ? Order.ascending
                        : Order.descending;

        this.selectedCurrency = this.currencies[0];
        this.groups = await this.groupService.getTotals(this.selectedCurrency.id, order, this.filter).toPromise();

        this.noResult = this.groups.length <= 1;
      }
    } catch (ex) {
      this.errorHandlering.handle(ex);
      this.noResult = true;
    } finally {
      this.setIsLoading(false);
    }
  }

  private sortGroups(groups: GroupTotal[]): GroupTotal[] {
      const direction = this.sort.active && this.sort.direction === 'desc'
              ? -1
              : 1;

      return groups.sort((a1, a2) => a1.name.localeCompare(a2.name) * direction);
  }

  private setIsLoading(isLoading: boolean): void {
    setTimeout(() => this.isLoadingResults = isLoading);
  }
}
