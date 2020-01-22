using BlazAdmin;
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
        private readonly IQuickStartStepRepository quickStartStepRepository;
        private readonly IComponentRepository componentRepository;

        public ProductService(IProductRepository productRepository, IQuickStartStepRepository quickStartStepRepository, IComponentRepository componentRepository)
        {
            this.productRepository = productRepository;
            this.quickStartStepRepository = quickStartStepRepository;
            this.componentRepository = componentRepository;
        }

        public Task UpdateIntroductionAsync(int id, string introduction)
        {
            var product = productRepository.QueryByKey(id);
            if (product == null)
            {
                throw new OperationException("产品不存在");
            }
            product.Introduction = introduction;
            return productRepository.SaveChangesAsync();
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
                    NugetPackageName = x.NugetPackageName,
                    Introduction = x.Introduction
                };
                var latestVersion = x.ProductVersions.OrderByDescending(x => x.PublishTime).FirstOrDefault();
                productModel.Version = latestVersion?.Version;
                productModel.PublishDate = latestVersion?.PublishTime;
                productModel.ChangeLog = latestVersion?.ChangeLog;
                return productModel;
            }).ToList();
        }

        public IEnumerable<Repository.Model.Component> GetComponents(int versionId)
        {
            return componentRepository.GetComponents(versionId);
        }

        public Task UpdateQuickStartStepAsync(UpdateQuickStartStepModel updateModel)
        {
            var step = quickStartStepRepository.QueryByKey(updateModel.Id);
            if (step == null)
            {
                throw new OperationException("该步骤不存在");
            }
            step.Title = updateModel.Title;
            step.Description = updateModel.Content;
            return quickStartStepRepository.SaveChangesAsync();
        }

        public Task CreateQuickStartStepAsync(int productId, int productVersionId, UpdateQuickStartStepModel updateModel)
        {
            var steps = quickStartStepRepository.GetSteps(productVersionId);
            return quickStartStepRepository.CreateAsync(new QuickStartStep()
            {
                Description = updateModel.Content,
                ProductVersionId = productVersionId,
                Sort = steps.Any() ? steps.Max(x => x.Sort) + 1 : 0,
                Title = updateModel.Title
            });
        }

        public Task PublishAsync(int id)
        {
            var preVersion = productRepository.GetProductWithVersion(id).ProductVersions.OrderByDescending(x => x.Id).FirstOrDefault();
            if (preVersion == null)
            {
                throw new OperationException("没有任何版本");
            }
            if (preVersion.IsPublish)
            {
                throw new OperationException("最新版本已发布，不能再次发布");
            }
            preVersion.IsPublish = true;
            return productRepository.SaveChangesAsync();
        }

        public List<ProductVersion> GetProductVersions(int productId)
        {
            return productRepository.GetProductWithVersion(productId).ProductVersions.ToList();
        }

        public List<QuickStartStep> GetProductQuickStartSteps(int productVersionId)
        {
            return quickStartStepRepository.GetSteps(productVersionId);
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

        public async Task StepMoveDownAsync(int productId, int selectedVersionId, int id)
        {
            var steps = quickStartStepRepository.GetSteps(selectedVersionId);
            var currentStep = steps.FirstOrDefault(x => x.Id == id);
            if (currentStep == null)
            {
                throw new OperationException("当前步骤不存在");
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var currentIndex = steps.IndexOf(currentStep);
                var newIndex = currentIndex++;
                steps.Remove(currentStep);
                steps.Insert(++newIndex, currentStep);
                ResetSort(steps);
                await quickStartStepRepository.SaveChangesAsync();
                scope.Complete();
            }
        }

        public async Task DeleteStepAsync(int productVersionId, int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await quickStartStepRepository.DeleteAsync(id);
                var steps = quickStartStepRepository.GetSteps(productVersionId);
                ResetSort(steps);
                await quickStartStepRepository.SaveChangesAsync();
                scope.Complete();
            }
        }

        public async Task StepMoveUpAsync(int productId, int selectedVersionId, int id)
        {
            var steps = quickStartStepRepository.GetSteps(selectedVersionId);
            var currentStep = steps.FirstOrDefault(x => x.Id == id);
            if (currentStep == null)
            {
                throw new OperationException("当前步骤不存在");
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var currentIndex = steps.IndexOf(currentStep);
                var newIndex = currentIndex--;
                steps.Remove(currentStep);
                steps.Insert(--newIndex, currentStep);
                ResetSort(steps);
                await quickStartStepRepository.SaveChangesAsync();
                scope.Complete();
            }
        }

        private void ResetSort(List<QuickStartStep> steps)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                steps[i].Sort = i;
            }
        }

        public async Task PrePublishAsync(PublishVersionModel versionModel)
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
                    var oldComponents = productVersion.ComponentVersions.Select(x => new Repository.Model.Component()
                    {
                        ExportedTypes = x.Component.ExportedTypes
                    }).ToList();
                    var files = Directory.GetFiles(directory, "*.dll");
                    InitilizeAssemblies(productVersion, files);
                    var newExportedTypes = productVersion.ComponentVersions.SelectMany(x => x.Component.ExportedTypes).ToList();
                    foreach (var oldComponent in oldComponents)
                    {
                        var newComponent = productVersion.ComponentVersions.FirstOrDefault(x => x.Component.Id == oldComponent.Id)?.Component;
                        if (newComponent == null)
                        {
                            continue;
                        }
                        foreach (var oldExportedType in oldComponent.ExportedTypes)
                        {
                            var newExportedType = newComponent.ExportedTypes.Where(x => x.Name == oldExportedType.Name)
                                .FirstOrDefault(x => x.Namespace == oldExportedType.Namespace);
                            newExportedType.Component = newComponent;
                        }
                    }
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
                productVersion = new ProductVersion()
                {
                    PublishTime = DateTime.Now,
                    Version = versionModel.Version,
                    ChangeLog = versionModel.ChangeLog
                };
                product.ProductVersions.Add(productVersion);
                await productRepository.SaveChangesAsync();
            }
            else
            {
                var prevVersion = product.ProductVersions.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Id < productVersion.Id);
                productVersion.ExportedTypes.Clear();
                if (prevVersion != null)
                {
                    productVersion.QueryStartSteps = prevVersion.QueryStartSteps.ToList();
                    productVersion.ComponentVersions = prevVersion.ComponentVersions.Select(x => new ComponentVersion()
                    {
                        Component = x.Component,
                        ProductVersion = productVersion
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
                var types = Assembly.LoadFile(file).GetExportedTypes().Where(x => x.IsPublic)
                    .ToList();
                foreach (var type in types)
                {
                    var typeDescription = GetClassSummary(members, type);
                    var exportedType = new ExportedType
                    {
                        Description = typeDescription,
                        Name = Regex.Replace(type.Name, @"\`\d+", string.Empty),
                        Namespace = type.Namespace
                    };
                    if (type.IsGenericType)
                    {
                        exportedType.IsGeneric = true;
                        exportedType.GenericParameters = type.GetGenericArguments().Select(x => new ExportedTypeGenericParameter()
                        {
                            Name = Regex.Replace(x.Name, @"\`\d+", string.Empty),
                            Description = GetClassSummary(members, x),
                            ExportedType = exportedType
                        }).ToList();
                    }
                    productVersion.ExportedTypes.Add(exportedType);
                    InitilizeTypes(type, exportedType, members);
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

        private void InitilizeTypes(Type type, ExportedType exportedType, IDictionary<string, string> members)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var parameterAttribute = property.GetCustomAttribute<ParameterAttribute>();
                var cascadingParameterAttribute = property.GetCustomAttribute<CascadingParameterAttribute>();
                var exportedProperty = new ExportedProperty()
                {
                    Description = GetPropertySummary(members, property),
                    IsComponentCascadingParameter = cascadingParameterAttribute != null,
                    IsComponentParameter = parameterAttribute != null,
                    Name = Regex.Replace(property.Name, @"`\d+", string.Empty),
                    PropertyType = property.PropertyType.Name
                };
                exportedType.Properties.Add(exportedProperty);
            }
            if (!type.IsGenericType)
            {
                return;
            }
            var genericArgs = type.GetGenericArguments();
            foreach (var genericArg in genericArgs)
            {
                var exportedTypeGenericParameter = new ExportedTypeGenericParameter()
                {
                    ExportedType = exportedType,
                    Name = genericArg.Name
                };
                exportedType.GenericParameters.Add(exportedTypeGenericParameter);
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
