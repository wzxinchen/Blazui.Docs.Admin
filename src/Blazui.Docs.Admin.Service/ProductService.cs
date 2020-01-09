using BlazAdmin;
using Blazui.Component;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Blazui.Docs.Admin.Service
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public List<ProductModel> GetProducts()
        {
            var products = productRepository.QueryAll();
            return products.Select(x => new ProductModel()
            {
                Description = x.Description,
                GitHub = x.GitHub,
                NugetPackageName = x.NugetPackageName
            }).ToList();
        }

        public async Task CreateProductAsync(CreateProductModel productModel)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var products = productRepository.QueryAll();
                if (products.Any(x => x.NugetPackageName.Equals(productModel.NugetPackageName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new OperationException($"Nuget 包 {productModel.NugetPackageName} 已存在");
                }
                await productRepository.CreateAsync(new Repository.Model.Product()
                {
                    Description = productModel.Description,
                    GitHub = productModel.GitHub,
                    NugetPackageName = productModel.NugetPackageName
                });
                scope.Complete();
            }
        }
    }
}
