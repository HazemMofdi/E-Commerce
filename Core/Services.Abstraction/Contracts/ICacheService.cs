using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface ICacheService
    {
        Task<string?> GetCachedValueAsync(string key);
        Task SetCacheValueAsync(string key, object value, TimeSpan timeToLive);
    }
}
