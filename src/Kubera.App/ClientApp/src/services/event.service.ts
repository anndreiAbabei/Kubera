import { EventEmitter, Injectable } from '@angular/core';
import { Currency } from 'src/models/currency.model';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  public transactions = new EventEmitter();
  public refreshRequested = new EventEmitter();
  public updateTransaction = new EventEmitter();
  public selectedCurrencyChanged = new EventEmitter<Currency>();
}
