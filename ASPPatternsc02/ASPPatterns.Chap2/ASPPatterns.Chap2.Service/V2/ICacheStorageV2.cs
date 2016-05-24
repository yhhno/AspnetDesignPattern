using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPPatterns.Chap2.Service.V2
{
    public interface ICacheStorageV2
    {
        void Remove(string key);
        void Store(string key, object data);
        T Retrieve<T>(string key);
    }
}
