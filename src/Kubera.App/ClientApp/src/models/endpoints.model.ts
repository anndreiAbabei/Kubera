export class EndpointsGet {
  transactions = `${this.base}/transaction`;
  wallet = `${this.base}/wallet`;
  assets = `${this.base}/asset`;
  user = `${this.base}/user`;
  assetsTotal = `${this.base}/asset/totals`;
  currencies = `${this.base}/currency`;
  group = `${this.base}/group`;
  groupTotal = `${this.base}/group/totals`;

  constructor(private base: string) { }
}

export class EndpointsPost {
  transaction = `${this.base}/transaction`;

  constructor(private base: string) { }
}

export class EndpointsPatch {
  userCurrency = `${this.base}/user/currency`;

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
  patch = new EndpointsPatch(this.base);
  delete = new EndpointsDelete(this.base);
}
