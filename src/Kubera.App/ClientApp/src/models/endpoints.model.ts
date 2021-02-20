export class EndpointsGet {
  transactions = `${this.base}/transaction`;
  assets = `${this.base}/asset`;
  assetsTotal = `${this.base}/asset/totals`;
  currencies = `${this.base}/currency`;

  constructor(private base: string) { }
}

export class EndpointsPost {
  transaction = `${this.base}/transaction`;

  constructor(private base: string) { }
}

export class EndpointsPut {
  transaction = `${this.base}/transaction`;

  constructor(private base: string) { }
}

export class EndpointsDelete {
  transaction = `${this.base}/transaction`;

  constructor(private base: string) { }
}

export class Endpoints {
  base = '/api/v1';

  get = new EndpointsGet(this.base)
  post = new EndpointsPost(this.base);
  put = new EndpointsPut(this.base);
  delete = new EndpointsDelete(this.base);
}
