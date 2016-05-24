using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPPatterns.Chap2.Service.V2
{

    public class ProductRepositoryV2 : IProductRepositoryV2
    {
        public IList<ProductV2> GetAllProductsInV2(int categoryId)
        {
            IList<ProductV2> productsV2 = new List<ProductV2>();
            //Database operation to populate productsV2
            return productsV2;
        } 
    }
}
