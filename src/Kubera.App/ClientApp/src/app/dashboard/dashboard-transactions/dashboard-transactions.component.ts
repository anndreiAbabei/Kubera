import { AfterViewInit, Component, Input, OnChanges, OnDestroy, SimpleChanges, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorHandlerService } from 'src/services/errorHandler.service';
import { EventService } from 'src/services/event.service';
import { Filter, Order, Paging } from 'src/models/filtering.model';
import { Transaction, TransactionsResponse } from 'src/models/transactions.model';
import { TransactionsService } from 'src/services/transactions.service';
import { DashboardEditTransactionComponent } from '../dashboard-edit-transaction/dashboard-edit-transaction.component';

@Component({
    selector: 'app-dashboard-transactions',
    templateUrl: './dashboard-transactions.component.html',
    styleUrls: ['./dashboard-transactions.component.scss']
})
export class DashboardTransactionsComponent implements AfterViewInit, OnChanges, OnDestroy {
  public resultsLength = 0;
  public isLoadingResults = false;
  public noResult = false;
  public displayedColumns: string[] = ['createdAt', 'group', 'asset', 'wallet', 'action', 'amount', 'rate', 'totalFormated', 'feeFormated', 'menu'];
  public canLoadMore = true;
  public transactions: Transaction[];
  public page: Paging;
  public readonly itemsPerPage = 30;

  @Input()
  public filter: Filter;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private readonly transactionService: TransactionsService,
    private readonly modalService: NgbModal,
    private readonly errorHandlerService: ErrorHandlerService,
    private readonly eventService: EventService) {  }

  public async ngAfterViewInit(): Promise<void> {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.eventService.transactions.subscribe(async () => await this.refreshTransactions());

    await this.refreshTransactions();
  }

  public async ngOnChanges(changes: SimpleChanges): Promise<void> {
    if (!changes['filter'].firstChange) {
      await this.refreshTransactions();
    }
  }

  public ngOnDestroy(): void {
    this.eventService.transactions.unsubscribe();
  }

  public async addTransaction(): Promise<void> {
    if (this.isLoadingResults) {
      return;
    }

    const result = await this.modalService.open(DashboardEditTransactionComponent).result;

    if (!result) {
      return;
    }

    try {
      this.isLoadingResults = true;
      const t = await this.transactionService.create(result).toPromise();
      this.transactions.push(t);
      this.eventService.updateTransaction.emit(t);
    } catch (ex) {
      this.errorHandlerService.handle(ex);
    } finally {
      this.isLoadingResults = false;
    }
  }

  public async showTransactionEdit(transaction: Transaction): Promise<void> {
    const reference = this.modalService.open(DashboardEditTransactionComponent);

    reference.componentInstance.transaction = transaction;
    const result = await reference.result;

    if (!result) {
      return;
    }

    try {
      this.isLoadingResults = true;
      await this.transactionService.update(result).toPromise();
      this.eventService.updateTransaction.emit(result);
    } catch (ex) {
      this.errorHandlerService.handle(ex);
    } finally {
      this.isLoadingResults = false;
    }
  }

  public async refreshTransactions(): Promise<void> {
    try {
      this.setIsLoading(true);
      const response = await this.getTransactions();

      this.transactions = this.parseResults(response);
    } catch (error) {
      this.errorHandlerService.handle(error);
    } finally {
      this.noResult = !this.transactions || this.transactions.length === 0;
      this.setIsLoading(false);
    }
  }

  public async removeTransaction(transaction: Transaction): Promise<void> {
    if (this.isLoadingResults || !transaction) {
      return;
    }

    const answer = confirm(`Are you sure you want to remove ${transaction.action} transaction of ${transaction.amount} ${transaction.asset.code} from ${transaction.createdAt}?`);

    if (!answer) {
      return;
    }

    this.isLoadingResults = true;

    try {
      await this.transactionService.delete(transaction.id).toPromise();
      this.transactions = this.transactions.filter(t => t.id !== transaction.id);
      this.eventService.updateTransaction.emit();
    } catch (ex) {
      this.errorHandlerService.handle(ex);
    } finally {
      this.isLoadingResults = false;
    }
  }

  public formatTotal(transaction: Transaction): string {
    return transaction.totalFormated;
  }

  public async performRefresh(): Promise<void> {
    this.eventService.refreshRequested.emit();
    await this.refreshTransactions();
  }

  private async getTransactions(): Promise<TransactionsResponse> {
    const order = this.sort.active && this.sort.direction === 'asc'
      ? Order.ascending
      : Order.descending;

    const page = new Paging();
    page.page = this.paginator.pageIndex;
    page.items = this.itemsPerPage;

    return await this.transactionService.getAll(page, order, this.filter).toPromise();
  }

  private parseResults(data: TransactionsResponse): Transaction[] {
    this.resultsLength = data.totalItems > 0 ? Math.ceil(data.totalItems / this.itemsPerPage) : 0;

    data.transactions.forEach(t => {
      t.totalFormated = `${t.rate * t.amount} ${t.currency?.symbol}`;
      t.feeFormated = t.fee
        ? `${t.fee} ${t.feeCurrency?.symbol}`
        : `${0.00} ${t.currency?.symbol}`;
      t.action = t.amount < 0 ? 'SOLD' : 'BOUGHT';
    });

    return data.transactions;
  }

  private setIsLoading(isLoading: boolean): void {
    setTimeout(() => this.isLoadingResults = isLoading);
  }
}
