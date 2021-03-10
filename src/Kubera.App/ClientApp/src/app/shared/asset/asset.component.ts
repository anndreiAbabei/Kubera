import { Component, Input } from '@angular/core';
import { Asset } from 'src/models/asset.model';

@Component({
    selector: 'app-asset',
    templateUrl: './asset.component.html',
    styleUrls: ['./asset.component.scss']
})
/** asset component*/
export class AssetComponent {
  @Input()
  public asset: Asset;
  @Input()
  public forceShowName: boolean;
}
