import { Component, Input } from '@angular/core';
import { Currency } from 'src/models/currency.model';

@Component({
    selector: 'app-currency',
    templateUrl: './currency.component.html',
    styleUrls: ['./currency.component.scss']
})
export class CurrencyComponent {
  @Input()
  public currency: Currency;
  @Input()
  public amount: number;
  @Input()
  public hideAmount: boolean;
  @Input()
  public forceBuble: boolean;
}
