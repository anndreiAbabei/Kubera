export interface ICacheHandler {
  has(key: string): boolean;

  store(key: string, value: any): void;

  get(key: string): any;
}

export class MemoryCacheHandler implements ICacheHandler {
  private static readonly storage: Map<string, any> = new Map<string, any>();

  public get(key: string): any {
    return MemoryCacheHandler.storage.get(key);
  }

  public has(key: string): boolean {
    return MemoryCacheHandler.storage.has(key);
  }

  public store(key: string, value: any): void {
    MemoryCacheHandler.storage.set(key, value);
  }
}

export class PersistentCacheHandler implements ICacheHandler {

  public get(key: string): any {
    return localStorage.getItem(key);
  }

  public has(key: string): boolean {
    return localStorage.getItem(key) !== null;
  }

  public store(key: string, value: any): void {
    localStorage.setItem(key, value);
  }
}
