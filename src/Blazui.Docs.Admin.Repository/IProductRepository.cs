using Blazui.Docs.Admin.Repository.Model;
using System;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<int> UpdateAsync(Product product);
    }
}
