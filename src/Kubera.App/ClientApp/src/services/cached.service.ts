import { Observable, of } from 'rxjs';
import { ICacheHandler, MemoryCacheHandler, PersistentCacheHandler } from './cache.handler';

export class CachedService {
  protected memoryCache: ICacheHandler = new MemoryCacheHandler();
  protected persistentCache: ICacheHandler = new PersistentCacheHandler();

  protected getOrAddInCache<T>(key: string, handler: () => Observable<T>, cache?: ICacheHandler): Observable<T> {
    cache = cache ?? this.memoryCache;

    if (cache.has(key)) {
      return of(cache.get(key));
    }

    const obs = handler();

    obs.subscribe(r => cache.store(key, r));

    return obs;
  }
}
