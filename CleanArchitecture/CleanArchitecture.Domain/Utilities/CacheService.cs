using CleanArchitecture.Domain.Utilities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Utilities
{
    public class CacheService : ICacheService
    {
        private readonly ObjectCache _memoryCache = MemoryCache.Default;

        public T GetData<T>(string key)
        {
            T data = (T)_memoryCache.Get(key);
            //TODO: JSON decode for value.....
            return data;
        }

        public object RemoveData(string key)
        {
            var removedData = _memoryCache.Remove(key);
            return removedData;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expires)
        {
            if (string.IsNullOrEmpty(key)) return false;
            //TODO: JSON encode for value.....
            _memoryCache.Set(key, value, expires);
            return true;
        }
    }
}
