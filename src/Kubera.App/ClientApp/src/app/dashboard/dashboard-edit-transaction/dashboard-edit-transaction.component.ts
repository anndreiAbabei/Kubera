import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Asset } from 'src/models/asset.model';
import { Currency } from 'src/models/currency.model';
import { Transaction } from 'src/models/transactions.model';
import { AssetService } from 'src/services/asset.service';
import { CurrencyService } from 'src/services/currency.service';
import { ErrorHandlerService } from 'src/services/errorHandler.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard-edit-transaction.component.html',
  styleUrls: ['./dashboard-edit-transaction.component.scss']
})
export class DashboardEditTransactionComponent implements OnInit {
  public transaction: Transaction;
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
    private readonly errorHandlerService: ErrorHandlerService) {

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

  public async ngOnInit(): Promise<void> {
    this.addTransactionForm.get('fee').disable();
    this.addTransactionForm.get('feeCurrency').disable();

    try {
      this.assets = await this.assetsService.getAll().toPromise();
      this.currencies = await this.currencyService.getAll().toPromise();
    } catch (ex) {
      this.errorHandlerService.handle(ex);
      this.activeModal.close();
    }

    if (this.transaction) {
      this.addTransactionForm.get('date').setValue(this.transaction.createdAt);
      this.addTransactionForm.get('asset').setValue(this.transaction.assetId);
      this.addTransactionForm.get('wallet').setValue(this.transaction.wallet);
      this.addTransactionForm.get('amount').setValue(this.transaction.amount);
      this.addTransactionForm.get('rate').setValue(this.transaction.rate);
      this.addTransactionForm.get('currency').setValue(this.transaction.currencyId);

      if (this.transaction.fee) {
        this.addTransactionForm.get('fee').setValue(this.transaction.fee);
        this.addTransactionForm.get('feeCurrency').setValue(this.transaction.feeCurrencyId);
      }
    } else {
      this.addTransactionForm.get('date').setValue(new Date(Date.now()));
    }
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

    if (!this.transaction) {
      this.transaction = new Transaction();
    }

    this.transaction.createdAt = this.addTransactionForm.get('date').value;
    this.transaction.assetId = this.addTransactionForm.get('asset').value;
    this.transaction.wallet = this.addTransactionForm.get('wallet').value;
    this.transaction.amount = this.addTransactionForm.get('amount').value;
    this.transaction.rate = this.addTransactionForm.get('rate').value;
    this.transaction.currencyId = this.addTransactionForm.get('currency').value;

    if (this.haveFee) {
      this.transaction.fee = this.addTransactionForm.get('fee').value;
      this.transaction.feeCurrencyId = this.addTransactionForm.get('feeCurrency').value;
    }

    this.activeModal.close(this.transaction);
  }
}
