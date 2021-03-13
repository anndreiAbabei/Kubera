import { Asset } from './asset.model';

export class AssetTotal extends Asset {
    sumAmount: number;
    total: number;
    totalNow: number;
    increase: number;
}

export class AssetTotals {
  public static Empty = <AssetTotals>{
    assets: [],
    count: 0,
    increase: 0,
    total: 0,
    totalNow: 0
  };

  assets: AssetTotal[];
  count: number;
  total: number;
  totalNow: number;
  increase: number;
}
