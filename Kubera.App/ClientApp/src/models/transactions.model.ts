import { Asset } from './asset.model';
import { Currency } from './currency.model';
import { Group } from './group.model';

export class TransactionsResponse {
  totalItems: number;
  currentPage: number;
  itemsPerPage: number;

  transactions: Transaction[];
}

export class Transaction {
  id: string;
  assetId: string;
  wallet: string;
  createdAt: Date;
  amount: number;
  currencyId: string;
  rate: number;
  fee?: number;
  feeCurrencyId?: string;

  currency: Currency;
  group: Group;
  asset: Asset;
  feeCurrency: Currency;

  get totalFormated(): string{
    return `${this.rate * this.amount} ${this.currency.symbol}`;
  }

  get feeFormated(): string {
     return this.fee
        ? `${this.fee} ${this.currency.symbol}`
        : `${0.00} ${this.currency.symbol}`;
  }

  get action(): string {
    return this.amount < 0 ? 'SOLD' : 'BOUGHT';
  }
}
