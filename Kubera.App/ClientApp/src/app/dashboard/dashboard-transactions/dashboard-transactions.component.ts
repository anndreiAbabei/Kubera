import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
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
  public isLoadingResults = true;
  public noResult = false;
  public displayedColumns: string[] = ['createdAt', 'group', 'asset', 'wallet', 'amount', 'rate', 'totalFormated', 'action', 'feeFormated'];
  public canLoadMore = true;
  public transactions: Transaction[];
  public page: Paging;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private readonly transactionService: TransactionsService,
    private readonly modalService: NgbModal) {  }

  public ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(startWith({}), switchMap(() => {
        this.isLoadingResults = true;

        const order = this.sort.active && this.sort.direction === 'asc'
                        ? Order.ascending
                        : Order.descending;

        const page = new Paging();
        page.page = this.paginator.pageIndex;
        page.items = 30;

        return this.transactionService.getAll(page, order);
      }),
      map(data => {
        this.isLoadingResults = false;
        this.noResult = data.transactions.length === 0;
        this.resultsLength = data.totalItems;

        return data.transactions;
      }),
      catchError(() => {
        this.isLoadingResults = false;
        this.noResult = true;

        return observableOf([]);
      })).subscribe(data => this.transactions = data);
  }

  public addTransaction(): void {
    if (this.isLoadingResults) {
      return;
    }

    this.modalService.open(DashboardCreateTransactionComponent);
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
}
