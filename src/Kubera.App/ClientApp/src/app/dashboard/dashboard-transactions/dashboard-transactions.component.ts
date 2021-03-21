import { AfterViewInit, ChangeDetectorRef, Component, Input, OnChanges, OnDestroy, SimpleChanges, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorHandlerService } from 'src/services/errorHandler.service';
import { EventService } from 'src/services/event.service';
import { Filter, Order, Paging } from 'src/models/filtering.model';
import { Transaction, TransactionsResponse } from 'src/models/transactions.model';
import { TransactionsService } from 'src/services/transactions.service';
import { DashboardEditTransactionComponent } from '../dashboard-edit-transaction/dashboard-edit-transaction.component';
import { WalletService } from 'src/services/wallet.service';

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

  private wallets: string[];

  @Input()
  public filter: Filter;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private readonly transactionService: TransactionsService,
    private readonly walletService: WalletService,
    private readonly modalService: NgbModal,
    private readonly errorHandlerService: ErrorHandlerService,
    private readonly eventService: EventService,
    private readonly chDetRef: ChangeDetectorRef) {  }

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
    // this.eventService.transactions.unsubscribe();
  }

  public async addTransaction(): Promise<void> {
    if (this.isLoadingResults) {
      return;
    }

    const result = await this.showTransactionEditModal();

    if (!result) {
      return;
    }

    try {
      this.isLoadingResults = true;
      const t = await this.transactionService.create(result).toPromise();
      this.transactions = [t, ...this.transactions];
      this.eventService.updateTransaction.emit(t);
      this.addIfWalletIfNotExists(result.wallet);
      this.chDetRef.markForCheck();
    } catch (ex) {
      this.errorHandlerService.handle(ex);
    } finally {
      this.isLoadingResults = false;
    }
  }

  public async showTransactionEdit(transaction: Transaction): Promise<void> {
    if (this.isLoadingResults) {
      return;
    }

    const result = await this.showTransactionEditModal(transaction);

    if (!result) {
      return;
    }

    try {
      this.isLoadingResults = true;
      await this.transactionService.update(result).toPromise();
      this.eventService.updateTransaction.emit(result);
      this.addIfWalletIfNotExists(result.wallet);
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

    try {
      const response = await this.walletService.getAll().toPromise();

      if (!response.wallets || response.wallets.length <= 0) {
        this.wallets = this.getWalletsFrom(this.transactions);
      } else {
        this.wallets = response.wallets;
      }
    } catch {
      this.wallets = this.getWalletsFrom(this.transactions);
    }
  }

  public async removeTransaction(transaction: Transaction): Promise<void> {
    if (this.isLoadingResults || !transaction) {
      return;
    }

    const answer = confirm(`Are you sure you want to remove ${transaction.action} transaction of ${transaction.amount} ${transaction.asset.code} from ${transaction.date}?`);

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

    data.transactions.forEach(t => this.formatTransaction(t));

    return data.transactions;
  }

  private async showTransactionEditModal(transaction?: Transaction): Promise<Transaction> {
    const reference = this.modalService.open(DashboardEditTransactionComponent);

    reference.componentInstance.wallets = this.wallets || this.getWalletsFrom(this.transactions);

    if (transaction) {
      reference.componentInstance.transaction = transaction;
    }

    const result = await reference.result;

    this.formatTransaction(result);

    return result;
  }

  private getWalletsFrom(transactions: Transaction[]): string[] {
    return transactions.map(t => t.wallet).filter((value, index, self) => self.indexOf(value) === index);
  }

  private addIfWalletIfNotExists(wallet: string): string[] {
    if (!wallet) {
      return;
    }

    if (!this.wallets) {
      this.wallets = this.getWalletsFrom(this.transactions);
    }

    if (this.wallets.find(w => w === wallet)) {
      return;
    }

    this.wallets.unshift(wallet);
  }

  private formatTransaction(transaction: Transaction): void {
    if (!transaction) {
      return;
    }

    transaction.totalFormated = `${transaction.rate * transaction.amount} ${transaction.currency?.symbol}`;
    transaction.feeFormated = transaction.fee
      ? `${transaction.fee} ${transaction.feeCurrency?.symbol}`
      : `${0.00} ${transaction.currency?.symbol}`;
    transaction.action = transaction.amount < 0 ? 'SOLD' : 'BOUGHT';
  }

  private setIsLoading(isLoading: boolean): void {
    setTimeout(() => this.isLoadingResults = isLoading);
  }
}
