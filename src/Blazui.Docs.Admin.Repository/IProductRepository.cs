using Blazui.Docs.Admin.Repository.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<int> UpdateAsync(Product product);
        Task<Product> GetFullProductAsync(int id);
        List<Product> GetProductsWithVersion();
        Product GetProductWithVersion(int productId);
    }
}
