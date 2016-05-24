using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPPatterns.Chap2.Service.V2
{
    // 如果不希望在ProductService类中缓存数据，Null Object模式就可以派上用场，
    //可以将NullObjectCache传递给ProductServiceV2，当请求缓存数据时，它什么也不做，而且总是向ProudctServiceV2返回Null，确保不会缓存任何数据
    public class NullObjectCache:ICacheStorage
    {
        public void Remove(string key)
        {
            
        }

        public void Store(string key, object data)
        {
            
        }

        public T Retrieve<T>(string storageKey)
        {
            return default(T);
        }
    }
}
