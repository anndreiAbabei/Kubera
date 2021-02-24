export class Paging {
  page: number;
  items: number;
}

export class DateFilter {
  from?: Date;
  to?: Date;
}

export enum Order {
  ascending = 1,
  descending = 2
}
