export class EndpointsGet {
  transactions = `${this.base}/transaction`;

  constructor(private base: string) { }
}

export class EndpointsDelete {
  transaction = `${this.base}/transaction`;

  constructor(private base: string) { }
}

export class Endpoints {
  base = '/api/v1';

  get = new EndpointsGet(this.base)
  delete = new EndpointsDelete(this.base);
}
