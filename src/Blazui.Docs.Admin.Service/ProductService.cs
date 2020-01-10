using BlazAdmin;
using Blazui.Component;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Repository;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
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
                Id = x.Id,
                NugetPackageName = x.NugetPackageName
            }).ToList();
        }

        public Task PublishAsync(PublishVersionModel versionModel)
        {
            var folder = Path.GetRandomFileName();
            var directory = Path.Combine(AppContext.BaseDirectory, folder);
            Directory.CreateDirectory(directory);
            ZipFile.ExtractToDirectory(versionModel.FilePackage.Id, directory);
            var subDirectories = Directory.GetDirectories(directory);
            foreach (var subDirectory in subDirectories)
            {
                if (subDirectory.EndsWith("package", StringComparison.CurrentCultureIgnoreCase))
                {
                    var files = Directory.GetFiles(subDirectory, "*.dll");
                    foreach (var file in files)
                    {
                        var componentTypes = Assembly.LoadFile(file).GetExportedTypes().Where(x => x.IsPublic)
                            .Where(x => typeof(ComponentBase).IsAssignableFrom(x))
                            .ToList();
                        foreach (var componentType in componentTypes)
                        {

                        }
                    }
                }
            }
        }

        public async Task CreateProductAsync(CreateProductModel productModel)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var products = productRepository.QueryAll();
                if (products.Any(x => x.NugetPackageName.Equals(productModel.NugetPackageName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new OperationException($"Nuget 包 {productModel.NugetPackageName} 已被另一个产品使用");
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

        public async Task UpdateProductAsync(CreateProductModel productModel)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var products = productRepository.QueryAll();
                if (products.Count(x => x.NugetPackageName.Equals(productModel.NugetPackageName, StringComparison.CurrentCultureIgnoreCase)) > 1)
                {
                    throw new OperationException($"Nuget 包 {productModel.NugetPackageName} 已被另一个产品使用");
                }
                await productRepository.UpdateAsync(new Repository.Model.Product()
                {
                    Id = productModel.Id,
                    Description = productModel.Description,
                    GitHub = productModel.GitHub,
                    NugetPackageName = productModel.NugetPackageName
                });
                scope.Complete();
            }
        }

        public async Task DeleteProductsAsync(params int[] ids)
        {
            foreach (var id in ids)
            {
                await productRepository.DeleteAsync(id);
            }
        }
    }
}
