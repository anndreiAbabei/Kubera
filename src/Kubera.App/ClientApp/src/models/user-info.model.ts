export class UserInfo {
  public email: string;
  public fullName: string;
  public settings: UserSettings;
}

export class UserSettings {
  public language: string;
  public prefferedCurrency: string;
  public currencies: string[];
}
