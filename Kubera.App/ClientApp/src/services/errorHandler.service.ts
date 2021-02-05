import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  public handle(error: any): void {
    console.log(error);
  }
}
