import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Asset } from 'src/models/asset.model';
import { Currency } from 'src/models/currency.model';
import { Transaction } from 'src/models/transactions.model';
import { AssetService } from 'src/services/asset.service';
import { CurrencyService } from 'src/services/currency.service';
import { ErrorHandlerService } from 'src/services/errorHandler.service';
import { TransactionsService } from 'src/services/transactions.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard-create-transaction.component.html',
  styleUrls: ['./dashboard-create-transaction.component.scss']
})
export class DashboardCreateTransactionComponent implements OnInit {
  public addTransactionForm: FormGroup;
  public assets: Asset[];
  public currencies: Currency[];
  public minAmount = 0.0000001;
  public minRate = 0.00000001;
  public minFee = 0.0000001;
  public haveFee = false;

  constructor(public readonly activeModal: NgbActiveModal,
    public readonly formBuilder: FormBuilder,
    private readonly assetsService: AssetService,
    private readonly currencyService: CurrencyService,
    private readonly errorHandlerService: ErrorHandlerService,
    private readonly transactionService: TransactionsService) {

    this.addTransactionForm = new FormGroup({
      date: new FormControl('', [
        Validators.required
      ]),
      asset: new FormControl('', [
        Validators.required
      ]),
      wallet: new FormControl('', [
        Validators.required
      ]),
      amount: new FormControl('', [
        Validators.required,
        Validators.min(this.minAmount)
      ]),
      rate: new FormControl('', [
        Validators.required,
        Validators.min(this.minRate)
      ]),
      currency:  new FormControl('', [
        Validators.required
      ]),
      fee: new FormControl('', [
        Validators.min(this.minFee)
      ]),
      feeCurrency: new FormControl('', [
        Validators.required
      ])
    });
  }

  public ngOnInit(): void {
    this.assetsService.getAll()
      .subscribe(a => this.assets = a, e => this.errorHandlerService.handle(e));

      this.currencyService.getAll()
        .subscribe(c => this.currencies = c, e => this.errorHandlerService.handle(e));

    this.addTransactionForm.get('fee').disable();
    this.addTransactionForm.get('feeCurrency').disable();
  }

  public toggleFee(): void {
    this.haveFee = !this.haveFee;
    const fee = this.addTransactionForm.get('fee');
    const feeCurrency = this.addTransactionForm.get('feeCurrency');

    if (this.haveFee) {
      fee.enable();
      feeCurrency.enable();
    } else  {
      fee.disable();
      feeCurrency.disable();
    }
  }

  public save(): void {
    if (!this.addTransactionForm.valid) {
      return;
    }

    const transaction = {
      createdAt: this.addTransactionForm.get('date').value,
      assetId: this.addTransactionForm.get('asset').value,
      wallet: this.addTransactionForm.get('wallet').value,
      amount: this.addTransactionForm.get('amount').value,
      rate: this.addTransactionForm.get('rate').value,
      currencyId: this.addTransactionForm.get('currency').value
    } as Transaction;

    if (this.haveFee) {
      transaction.fee = this.addTransactionForm.get('currency').value;
      transaction.feeCurrencyId = this.addTransactionForm.get('feeCurrency').value;
    }

    this.transactionService.create(transaction)
      .subscribe(t => this.activeModal.close(t), e => this.errorHandlerService.handle(e));
  }
}
