using BlazAdmin;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Repository;
using Blazui.Docs.Admin.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Blazui.Docs.Admin.Service
{
    public class ComponentService
    {
        private readonly IComponentRepository componentRepository;
        private readonly IExportedTypeRepository exportedTypeRepository;
        private readonly IProductVersionRepository productVersionRepository;

        public ComponentService(IComponentRepository componentRepository, IExportedTypeRepository exportedTypeRepository, IProductVersionRepository productVersionRepository)
        {
            this.componentRepository = componentRepository;
            this.exportedTypeRepository = exportedTypeRepository;
            this.productVersionRepository = productVersionRepository;
        }

        public List<AdminComponentModel> GetComponents(int productId, int productVerionsId)
        {
            var components = componentRepository.GetComponents(productVerionsId);
            var adminComponents = new List<AdminComponentModel>();
            foreach (var component in components)
            {
                adminComponents.Add(new AdminComponentModel()
                {
                    Id = component.Id,
                    Description = component.Description,
                    Name = component.Name,
                    TagName = component.TagName,
                    AddAtVersion = component.Versions.OrderByDescending(x => x.ProductVersion.PublishTime).FirstOrDefault()?.ProductVersion.Version
                });
            }
            return adminComponents;
        }

        public Task<List<ExportedType>> GetExportedTypesAsync(int productVersionId)
        {
            return exportedTypeRepository.GetExportedTypesAsync(productVersionId);
        }

        public async Task CreateComponentAsync(int productVersionId, ComponentEditModel component)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var exportedTypes = await exportedTypeRepository.GetExportedTypesAsync(productVersionId);
                var existsComponent = await componentRepository.GetComponentAsync(component.TagName);
                var productVersion = productVersionRepository.QueryByKey(productVersionId);
                var exportedTypeIds = component.ExportedTypeIds.Select(x => Convert.ToInt32(x)).ToArray();
                if (existsComponent == null)
                {
                    var newComponent = new Repository.Model.Component()
                    {
                        Description = component.Description,
                        ExportedTypes = exportedTypes.Where(x => exportedTypeIds.Contains(x.Id)).ToList(),
                        Name = component.Name,
                        TagName = component.TagName
                    };
                    foreach (var exportedType in newComponent.ExportedTypes)
                    {
                        exportedType.Component = newComponent;
                    }
                    newComponent.Versions.Add(new ComponentVersion()
                    {
                        Component = newComponent,
                        ProductVersion = productVersion
                    });
                    await componentRepository.CreateAsync(newComponent);
                }
                else if (existsComponent.Versions.Any(x => x.ProductVersionId == productVersionId))
                {
                    throw new OperationException("该组件在该版本已存在");
                }
                else
                {
                    existsComponent.ExportedTypes = exportedTypes.Where(x => exportedTypeIds.Contains(x.Id)).ToList();
                    foreach (var exportedType in existsComponent.ExportedTypes)
                    {
                        exportedType.Component = existsComponent;
                    }
                    existsComponent.Name = component.Name;
                    existsComponent.Versions.Add(new ComponentVersion()
                    {
                        Component = existsComponent,
                        ProductVersion = productVersion
                    });
                    await productVersionRepository.SaveChangesAsync();
                }
                scope.Complete();
            }
        }

        public async Task<List<ExportedType>> GetComponentExportedTypesAsync(int componentId, int productVersionId)
        {
            var component = await componentRepository.GetComponentAsync(componentId);
            if (component == null)
            {
                throw new OperationException("组件不存在");
            }
            return component.ExportedTypes.Where(x => x.ProductVersionId == productVersionId).ToList();
        }

        public async Task UpdateComponentAsync(int productVersionId, int componentId, ComponentEditModel component)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var existsComponent = await componentRepository.GetComponentAsync(componentId);
                if (existsComponent == null)
                {
                    throw new OperationException("组件不存在");
                }
                var exportedTypes = await exportedTypeRepository.GetExportedTypesAsync(productVersionId);
                var productVersion = await productVersionRepository.GetComponentsAsync(productVersionId);
                var exportedTypeIds = component.ExportedTypeIds.Select(x => Convert.ToInt32(x)).ToArray();
                if (productVersion.ComponentVersions.Any(x => x.Component.Id != componentId && (x.Component.Name == component.Name || x.Component.TagName == component.TagName)))
                {
                    throw new OperationException("该组件名称或调用标签在该版本已存在");
                }
                existsComponent.ExportedTypes = exportedTypes.Where(x => exportedTypeIds.Contains(x.Id)).ToList();
                foreach (var exportedType in existsComponent.ExportedTypes)
                {
                    exportedType.Component = existsComponent;
                }
                existsComponent.Name = component.Name;
                await productVersionRepository.SaveChangesAsync();
                scope.Complete();
            }
        }
    }
}
