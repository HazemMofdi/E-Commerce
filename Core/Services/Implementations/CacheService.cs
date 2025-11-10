using Domain.Contracts;
using Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public async Task<string?> GetCachedValueAsync(string key)
        {
            return await cacheRepository.GetAsync(key);
        }

        public async Task SetCacheValueAsync(string key, object value, TimeSpan timeToLive)
        {
            await cacheRepository.SetAsync(key, value, timeToLive);
        }
    }
}
