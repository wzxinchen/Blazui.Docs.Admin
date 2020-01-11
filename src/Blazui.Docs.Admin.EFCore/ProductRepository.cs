using Blazui.Docs.Admin.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository.EFCore
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> UpdateAsync(Product product)
        {
            var existsProduct = QueryByKey(product.Id);
            existsProduct.GitHub = product.GitHub;
            existsProduct.NugetPackageName = product.NugetPackageName;
            existsProduct.Description = product.Description;
            return await DbContext.SaveChangesAsync();
        }

        public Task<Product> GetFullProductAsync(int id)
        {
            return Query.Include(x => x.ProductVersions)
                .ThenInclude(x => x.Components)
                .ThenInclude(x => x.ComponentParameters)
                .FirstOrDefaultAsync(x => x.Id == id);


        }
    }
}
