export class EndpointsGet {
  transactions = `${this.base}/transaction`;
  assets = `${this.base}/asset`;
  currencies = `${this.base}/currency`;

  constructor(private base: string) { }
}

export class EndpointsPost {
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
  delete = new EndpointsDelete(this.base);
}