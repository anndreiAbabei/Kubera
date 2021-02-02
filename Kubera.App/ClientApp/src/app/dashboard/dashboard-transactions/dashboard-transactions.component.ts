import { Component, OnInit } from '@angular/core';
import { Paging } from '../../../models/filtering.model';
import { Transaction } from '../../../models/transactions.model';
import { TransactionsService } from '../../../services/transactions.service';

@Component({
    selector: 'app-dashboard-transactions',
    templateUrl: './dashboard-transactions.component.html',
    styleUrls: ['./dashboard-transactions.component.scss']
})
export class DashboardTransactionsComponent implements OnInit {
  public loading = false;
  public canLoadMore = true;
  public transactions: Transaction[];
  public page: Paging;

  constructor(private readonly transactionService: TransactionsService) {
    this.page = {
      page: 0,
      items: 10
    };
  }

  public ngOnInit(): void {
    this.loadMore(false);
  }

  public loadMore(increasePage?: boolean): void {
    if (this.loading || !this.canLoadMore)
      return;

    this.loading = true;
    if (increasePage)
      this.page.page++;

    this.transactionService.getAll(this.page)
      .subscribe(r => {
        if (!this.transactions)
          this.transactions = r.transactions;
        else
          this.transactions = this.transactions.concat(r.transactions);

        this.canLoadMore = r.totalPages > this.page.page;
        this.loading = false;
      });
  }

  public addTransaction(): void {
    if (this.loading)
      return;

  }

  public removeTransaction(transaction: Transaction): void {
    if (this.loading || !transaction)
      return;

    const answer = confirm('Are you sure you want to remove transaction [' + transaction.id + ']?');

    if (!answer)
      return;

    this.loading = true;
    this.transactionService.delete(transaction.id)
      .subscribe(() => {
        this.transactions = this.transactions.filter(t => t.id !== transaction.id);
        this.loading = false;
      });
  }
}
