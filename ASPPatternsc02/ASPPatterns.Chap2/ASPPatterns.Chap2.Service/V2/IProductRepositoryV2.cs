using System.Collections.Generic;

namespace ASPPatterns.Chap2.Service.V2
{
    public interface IProductRepositoryV2
    {
        IList<ProductV2> GetAllProductsInV2(int categoryId);
    }
}