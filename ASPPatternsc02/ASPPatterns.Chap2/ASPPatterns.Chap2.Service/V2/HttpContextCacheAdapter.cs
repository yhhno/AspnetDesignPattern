using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ASPPatterns.Chap2.Service.V2
{
    //v3 HttpContextCacheAdapter类，此类是HTTP Context缓存的包装器，并将工作委托给HTTP Context缓存的方法。
    public class HttpContextCacheAdapter:ICacheStorageV2
    {
        public void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        public void Store(string key, object data)
        {
            HttpContext.Current.Cache.Insert(key,data);
        }

        public T Retrieve<T>(string key)
        {
            T itemStored = (T) HttpContext.Current.Cache.Get(key);
            if (itemStored == null)
            {
                itemStored = default(T);
            }
            return itemStored;
        }
    }
}
