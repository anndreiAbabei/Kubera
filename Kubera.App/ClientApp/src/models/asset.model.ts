import { Group } from './group.model';

export class Asset {
  id: string;
  code: string;
  name: string;
  symbol: string;
  order: number;
  groupId: string;
  icon: string;

  group: Group;
}
