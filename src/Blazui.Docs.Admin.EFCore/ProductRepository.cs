﻿using Blazui.Docs.Admin.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .ThenInclude(x => x.ComponentVersions)
                .Include(x => x.ProductVersions)
                .ThenInclude(x => x.ExportedTypes)
                .ThenInclude(x => x.GenericParameters)
                .Include(x => x.ProductVersions)
                .ThenInclude(x => x.ExportedTypes)
                .ThenInclude(x => x.Properties)
                .FirstOrDefaultAsync(x => x.Id == id);


        }

        public List<Product> GetProductsWithVersion()
        {
            return Query.Include(x => x.ProductVersions).ToList();
        }

        public Product GetProductWithVersion(int id)
        {
            return Query.Include(x => x.ProductVersions)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
