import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  public transactions = new EventEmitter();
  public updateTransaction = new EventEmitter();
}
