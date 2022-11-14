using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace EntityPresentorProj.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public object GetValue(string key)
        {
            return _cache.Get(key);
            
        }
        public string GetValueAsString(string key)
        {
         var val = _cache.Get(key);
            if (val == null)return null;
            return Encoding.Default.GetString(_cache.Get(key));
        }
        public void SetStringValue(string key, string val)
        {
            //_cache.SetString(key,val);
            _cache.SetString(key, val);
        }
        public void SetValue(string key, byte[] val)
        {
            //_cache.SetString(key,val);
            _cache.Set(key, val);
        }
        

    }
}
