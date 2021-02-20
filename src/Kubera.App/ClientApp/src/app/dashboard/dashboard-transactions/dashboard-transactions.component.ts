import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { merge, of as observableOf } from 'rxjs';
import { map, startWith, switchMap, catchError } from 'rxjs/operators';
import { Order, Paging } from '../../../models/filtering.model';
import { Transaction } from '../../../models/transactions.model';
import { TransactionsService } from '../../../services/transactions.service';
import { DashboardCreateTransactionComponent } from '../dashboard-create-transaction/dashboard-create-transaction.component';

@Component({
    selector: 'app-dashboard-transactions',
    templateUrl: './dashboard-transactions.component.html',
    styleUrls: ['./dashboard-transactions.component.scss']
})
export class DashboardTransactionsComponent implements AfterViewInit {
  public resultsLength = 0;
  public isLoadingResults = false;
  public noResult = false;
  public displayedColumns: string[] = ['createdAt', 'group', 'asset', 'wallet', 'action', 'amount', 'rate', 'totalFormated', 'feeFormated'];
  public canLoadMore = true;
  public transactions: Transaction[];
  public page: Paging;
  public readonly itemsPerPage = 30;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private readonly transactionService: TransactionsService,
    private readonly modalService: NgbModal) {  }

  public ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    this.refreshTransactions();
  }

  public addTransaction(): void {
    if (this.isLoadingResults) {
      return;
    }

    this.modalService.open(DashboardCreateTransactionComponent)
      .result.then(p => {
        if (p) {
          this.transactions.push(p);
        }
      });
  }

  public refreshTransactions(): void {
    merge(this.sort.sortChange, this.paginator.page)
    .pipe(startWith({}), switchMap(() => {
      this.setIsLoading(true);

      const order = this.sort.active && this.sort.direction === 'asc'
                      ? Order.ascending
                      : Order.descending;

      const page = new Paging();
      page.page = this.paginator.pageIndex;
      page.items = this.itemsPerPage;

      return this.transactionService.getAll(page, order);
    }),
    map(data => {
      this.noResult = data.transactions.length === 0;
      this.resultsLength = data.totalItems > 0 ? Math.ceil(data.totalItems / this.itemsPerPage) : 0;

      data.transactions.forEach(t => {
        t.totalFormated = `${t.rate * t.amount} ${t.currency?.symbol}`;
        t.feeFormated = t.fee
                          ? `${t.fee} ${t.feeCurrency?.symbol}`
                          : `${0.00} ${t.currency?.symbol}`;
        t.action = t.amount < 0 ? 'SOLD' : 'BOUGHT';
        this.setIsLoading(false);
      });

      return data.transactions;
    }),
    catchError(() => {
      this.setIsLoading(false);
      this.noResult = true;

      return observableOf([]);
    })).subscribe(data => this.transactions = data);
  }

  public removeTransaction(transaction: Transaction): void {
    if (this.isLoadingResults || !transaction) {
      return;
    }

    const answer = confirm('Are you sure you want to remove transaction [' + transaction.id + ']?');

    if (!answer) {
      return;
    }

    this.isLoadingResults = true;
    this.transactionService.delete(transaction.id)
      .subscribe(() => {
        this.transactions = this.transactions.filter(t => t.id !== transaction.id);
        this.isLoadingResults = false;
      });
  }

  public formatTotal(transaction: Transaction): string {
    return transaction.totalFormated;
  }

  private setIsLoading(isLoading: boolean): void {
    setTimeout(() => this.isLoadingResults = isLoading);
  }
}
