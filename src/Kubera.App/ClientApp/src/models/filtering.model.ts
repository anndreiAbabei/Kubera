export class Paging {
  page: number;
  items: number;
}

export class Filter {
  from?: Date;
  to?: Date;
  assetId?: string;
  groupId?: string;
}

export enum Order {
  ascending = 1,
  descending = 2
}
