import { Group } from './group.model';

export class GroupTotal extends Group {
    sumAmount: number;
    total: number;
    totalNow: number;
    increase: number;
}

export class GroupTotals {
  public static Empty = <GroupTotals>{
    groups: [],
    count: 0,
    increase: 0,
    total: 0,
    totalNow: 0
  };

  groups: GroupTotal[];
  count: number;
  amount: number;
  total: number;
  totalNow: number;
  increase: number;
}
