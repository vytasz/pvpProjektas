using System;
using System.Runtime.Caching;

namespace WebApplication2.Services
{
    public class CacheService : ICacheService
    {
        private ObjectCache cache;

        public CacheService() //prisijungia prie cache'o
        {
            cache = MemoryCache.Default;
        }

        public bool AddItemToCache(string key, object value, int duration)
        {
            object keyInCache = cache.Get(key);
            if (keyInCache == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(duration)
                };
                cache.Set(key, value, policy);
                return true;
            }
            return false;
        }

        public object GetItemFromCache(string key) //paima daikta is cache
        {
            if (key.Equals("") || key.Equals(null)) return null;
            return cache.Get(key);
        }
    }
}