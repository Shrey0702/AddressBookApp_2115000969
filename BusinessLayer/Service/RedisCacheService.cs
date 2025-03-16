using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BusinessLayer.Service
{
    public class RedisCacheService: IRedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            var jsonData = JsonConvert.SerializeObject(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            await _cache.SetStringAsync(key, jsonData, options);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var jsonData = await _cache.GetStringAsync(key);
            return jsonData == null ? default : JsonConvert.DeserializeObject<T>(jsonData);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

    }
}
