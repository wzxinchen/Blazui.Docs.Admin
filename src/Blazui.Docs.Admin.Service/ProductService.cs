﻿using BlazAdmin;
using Blazui.Component;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Repository;
using Blazui.Docs.Admin.Repository.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;

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
            var products = productRepository.GetProductsWithVersion();
            return products.Select(x =>
            {
                var productModel = new ProductModel()
                {
                    Description = x.Description,
                    GitHub = x.GitHub,
                    Id = x.Id,
                    NugetPackageName = x.NugetPackageName
                };
                var latestVersion = x.ProductVersions.OrderByDescending(x => x.PublishTime).FirstOrDefault();
                productModel.Version = latestVersion?.Version;
                productModel.PublishDate = latestVersion.PublishTime;
                productModel.ChangeLog = latestVersion?.ChangeLog;
                return productModel;
            }).ToList();
        }

        public Task UpdateChangeLogAsync(int productId, string version, string changeLog)
        {
            var product = productRepository.GetProductWithVersion(productId);
            var productVersion = product.ProductVersions.FirstOrDefault(x => x.Version == version);
            if (productVersion == null)
            {
                throw new OperationException($"当前版本【{changeLog}】不存在");
            }
            productVersion.ChangeLog = changeLog;
            return productRepository.SaveChangesAsync();
        }

        public async Task PublishAsync(PublishVersionModel versionModel)
        {
            try
            {
                var folder = Path.GetRandomFileName();
                var directory = Path.Combine(AppContext.BaseDirectory, folder);
                Directory.CreateDirectory(directory);
                ZipFile.ExtractToDirectory(versionModel.FilePackage.FirstOrDefault().Id, directory);
                var subDirectories = Directory.GetDirectories(directory);
                var product = await productRepository.GetFullProductAsync(versionModel.ProductId);
                if (product == null)
                {
                    throw new OperationException("当前产品不存在，产品ID为：" + versionModel.ProductId);
                }
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var productVersion = await InitilizeVersionAsync(product, versionModel);
                    var files = Directory.GetFiles(directory, "*.dll");
                    InitilizeAssemblies(productVersion, files);
                    await productRepository.SaveChangesAsync();
                    scope.Complete();
                }
            }
            finally
            {
                File.Delete(versionModel.FilePackage[0].Id);
            }
        }

        private async Task<ProductVersion> InitilizeVersionAsync(Product product, PublishVersionModel versionModel)
        {
            var productVersion = product.ProductVersions.FirstOrDefault(x => x.Version.Equals(versionModel.Version, StringComparison.CurrentCultureIgnoreCase));
            if (productVersion == null)
            {
                productVersion = new Repository.Model.ProductVersion()
                {
                    PublishTime = DateTime.Now,
                    Version = versionModel.Version,
                    ChangeLog = versionModel.ChangeLog
                };
                product.ProductVersions.Add(productVersion);
                productVersion.Components.Clear();
                await productRepository.SaveChangesAsync();
            }
            else
            {
                var prevVersion = product.ProductVersions.FirstOrDefault(x => x.Id < productVersion.Id);
                productVersion.Components.Clear();
                if (prevVersion != null)
                {
                    productVersion.QueryStartSteps = prevVersion.QueryStartSteps.Select(x => new QuickStartStep()
                    {
                        Description = x.Description,
                        ProductVersionId = x.ProductVersionId,
                        Sort = x.Sort,
                        Title = x.Title
                    }).ToList();
                }
                productVersion.ChangeLog = versionModel.ChangeLog;
                await productRepository.SaveChangesAsync();
            }

            return productVersion;
        }

        private void InitilizeAssemblies(ProductVersion productVersion, string[] files)
        {
            foreach (var file in files)
            {
                var directory = Path.GetDirectoryName(file);
                var xmlFile = Path.Combine(directory, Path.GetFileNameWithoutExtension(file) + ".xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFile);
                var members = doc.SelectNodes("/doc/members/member")
                    .OfType<XmlNode>()
                    .ToDictionary(x => x.Attributes["name"].Value, x => x.SelectSingleNode("summary").InnerText?.Trim());
                var componentTypes = Assembly.LoadFile(file).GetExportedTypes().Where(x => x.IsPublic)
                    .Where(x => typeof(ComponentBase).IsAssignableFrom(x))
                    .Where(x => x != typeof(ComponentBase))
                    .ToList();
                foreach (var componentType in componentTypes)
                {
                    var componentName = GetClassSummary(members, componentType);
                    var tagName = Regex.Replace(componentType.Name, @"\`\d+", string.Empty);
                    var component = new Repository.Model.Component()
                    {
                        Name = componentName ?? tagName,
                        TagName = tagName,
                        ProductVersionId = productVersion.Id
                    };
                    if (componentType.IsGenericType)
                    {
                        component.ComponentGenericParameters = componentType.GetGenericArguments().Select(x => new ComponentGenericParameter()
                        {
                            Name = Regex.Replace(x.Name, @"\`\d+", string.Empty)
                        }).ToList();
                    }
                    productVersion.Components.Add(component);
                    InitilizeParameters(componentType, component, members);
                }
            }
        }

        string GetPropertySummary(IDictionary<string, string> members, PropertyInfo property)
        {
            var propertyFullName = property.DeclaringType.FullName + "." + property.Name;
            members.TryGetValue($"P:{propertyFullName}", out var value);
            return value;
        }

        string GetClassSummary(IDictionary<string, string> members, Type type)
        {
            members.TryGetValue($"T:{type.FullName}", out var value);
            return value;
        }

        private string GetFriendlyTypeName(Type type)
        {
            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            if (!type.IsGenericType)
            {
                return type.FullName;
            }

            var builder = new StringBuilder();
            var name = type.Name;
            var index = name.IndexOf("`");
            builder.AppendFormat("{0}.{1}", type.Namespace, name.Substring(0, index));
            builder.Append('<');
            var first = true;
            foreach (var arg in type.GetGenericArguments())
            {
                if (!first)
                {
                    builder.Append(',');
                }
                builder.Append(GetFriendlyTypeName(arg));
                first = false;
            }
            builder.Append('>');
            return builder.ToString();
        }

        private void InitilizeParameters(Type componentType, Repository.Model.Component component, IDictionary<string, string> members)
        {
            var properties = componentType.GetProperties();
            foreach (var property in properties)
            {
                var parameterAttribute = property.GetCustomAttribute<ParameterAttribute>();
                if (parameterAttribute != null)
                {
                    var componentParameter = new ComponentParameter()
                    {
                        IsCascading = false,
                        PropertyName = property.Name,
                        TypeName = property.PropertyType.FullName,
                        Description = GetPropertySummary(members, property)
                    };
                    component.ComponentParameters.Add(componentParameter);
                    continue;
                }
                var cascadingParameterAttribute = property.GetCustomAttribute<CascadingParameterAttribute>();
                if (cascadingParameterAttribute != null)
                {
                    var componentParameter = new ComponentParameter()
                    {
                        IsCascading = false,
                        PropertyName = property.Name,
                        TypeName = property.PropertyType.FullName,
                        Description = GetPropertySummary(members, property)
                    };
                    component.ComponentParameters.Add(componentParameter);
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
