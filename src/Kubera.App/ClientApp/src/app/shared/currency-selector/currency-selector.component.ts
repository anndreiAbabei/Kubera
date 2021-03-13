import { Component, Input, OnInit } from '@angular/core';
import { Currency } from 'src/models/currency.model';
import { CurrencyService } from 'src/services/currency.service';
import { EventService } from 'src/services/event.service';
import { UserService } from 'src/services/user.service';

@Component({
    selector: 'app-currency-selector',
    templateUrl: './currency-selector.component.html',
    styleUrls: ['./currency-selector.component.scss']
})
export class CurrencySelectorComponent implements OnInit {
  public currencies: Currency[];
  public selectedCurrencyId: string;
  public selectedCurrency: Currency;

  constructor(private readonly currencyService: CurrencyService,
    private readonly userService: UserService,
    private readonly eventService: EventService) {  }

  public async ngOnInit(): Promise<void> {
    this.currencies = await this.currencyService.getAll().toPromise();
    if (!this.currencies || this.currencies.length <= 0) {
      return;
    }

    const userInfo = await this.userService.getUserInfo().toPromise();

    if (userInfo?.settings?.prefferedCurrency) {
      for (const currency of this.currencies) {
        if (currency.id === userInfo.settings.prefferedCurrency) {
          this.selectedCurrencyId = currency.id;
          break;
        }
      }
    }

    if (!this.selectedCurrencyId) {
      this.selectedCurrencyId = this.currencies[0].id;
    }

    this.selectedCurrency =  this.currencies.find(c => c.id === this.selectedCurrencyId);
  }

  public async currencyChanged(arg: any): Promise<void> {
    this.selectedCurrency = this.currencies.find(c => c.id === arg.value);
    await this.userService.updateUserPrefferedCurrency(arg.value).toPromise();

    this.eventService.selectedCurrencyChanged.emit(this.selectedCurrency);
  }
}
