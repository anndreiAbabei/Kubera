import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-wallet',
    templateUrl: './wallet.component.html',
    styleUrls: ['./wallet.component.scss']
})
/** wallet component*/
export class WalletComponent {
  @Input()
  public wallet: string;

  public getIcon(wallet: string): string {
    return wallet && wallet.length > 0 ? wallet.substring(0, 1) : ' ';
  }
}
