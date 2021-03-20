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
  date: Date;
  amount: number;
  currencyId: string;
  rate: number;
  fee?: number;
  feeCurrencyId?: string;

  currency: Currency;
  group: Group;
  asset: Asset;
  feeCurrency: Currency;

  totalFormated: string;
  feeFormated: string;
  action: string;
}
