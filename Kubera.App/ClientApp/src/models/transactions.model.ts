export class TransactionsResponse {
  totalPages: number;
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
}
