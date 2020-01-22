using BlazAdmin;
using Blazui.Component;
using Blazui.Component.EventArgs;
using Blazui.Component;
using Blazui.Component.Select;
using Blazui.Docs.Admin.Enum;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Repository.Model;
using Blazui.Docs.Admin.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Pages.ComponentDocs
{
    public class ComponentsBase : BAdminPageBase
    {
        protected string EmptyMessage { get; set; } = "请选择产品及版本后查询";
        protected BForm form;
        private int? selectedVersionId;
        protected int? SelectedVersionId
        {
            get
            {
                return selectedVersionId;
            }
            set
            {
                selectedVersionId = value;
                if (value == null)
                {
                    EmptyMessage = "请选择产品及版本后查询";
                }
                Components.Clear();
                table.MarkAsRequireRender();
                RequireRender = true;
            }
        }
        protected bool CanCreate { get; set; }
        public bool CanUpdate { get; private set; }
        protected List<AdminComponentModel> Components { get; set; } = new List<AdminComponentModel>();
        protected BTable table;
        private int productId;
        private List<ProductVersion> productVersions;

        public Dictionary<int, string> Versions { get; private set; }
        [Inject]
        private ProductService ProductService { get; set; }

        [Inject]
        private ComponentService ComponentService { get; set; }
        public List<ProductModel> Products { get; private set; }

        protected void Query()
        {
            if (!form.IsValid())
            {
                return;
            }

            Components = ComponentService.GetComponents(productId, SelectedVersionId.Value);

            EmptyMessage = null;
            table.MarkAsRequireRender();
            RequireRender = true;
        }
        protected async Task EditAsync(object arg)
        {
            if (!form.IsValid())
            {
                return;
            }
            var adminComponentModel = arg as AdminComponentModel;
            var componentEditModel = new ComponentEditModel()
            {
                Description = adminComponentModel.Description,
                ExportedTypeIds = (await ComponentService.GetComponentExportedTypesAsync(adminComponentModel.Id, SelectedVersionId.Value)).Select(x => x.Id.ToString()).ToList(),
                Name = adminComponentModel.Name,
                TagName = adminComponentModel.TagName
            };
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(ComponentEdit.ProductId), productId);
            parameters.Add(nameof(ComponentEdit.ProductVersionId), SelectedVersionId.Value);
            parameters.Add(nameof(ComponentEdit.ComponentId), adminComponentModel.Id);
            parameters.Add(nameof(ComponentEdit.Component), componentEditModel);
            await DialogService.ShowDialogAsync<ComponentEdit>("更新组件", 800, parameters);
            Query();
        }
        protected async Task DeleteAsync(object arg)
        {
            var confirmResult = await ConfirmAsync("无法恢复，确认删除？");
            if (confirmResult != MessageBoxResult.Ok)
            {
                return;
            }
            var stepModel = arg as QuickStartStepModel;
            await ProductService.DeleteStepAsync(selectedVersionId.Value, stepModel.Id);
            Query();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Products = ProductService.GetProducts();
        }
        protected async Task CreateAsync()
        {
            if (!form.IsValid())
            {
                return;
            }
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(ComponentEdit.ProductId), productId);
            parameters.Add(nameof(ComponentEdit.ProductVersionId), SelectedVersionId.Value);
            await DialogService.ShowDialogAsync<ComponentEdit>("创建组件", 800, parameters);
            Query();
        }
        protected void ProductSelected(BChangeEventArgs<BSelectOptionBase<int?>> arg)
        {
            Components.Clear();
            table.MarkAsRequireRender();
            RequireRender = true;
            if (arg.NewValue.Value.HasValue)
            {
                productId = arg.NewValue.Value.Value;
                productVersions = ProductService.GetProductVersions(arg.NewValue.Value.Value);
                Versions = productVersions.ToDictionary(x => x.Id, x => x.Version);
            }
            else
            {
                EmptyMessage = "请选择产品及版本后查询";
                productId = 0;
                productVersions.Clear();
                Versions.Clear();
            }
        }
    }
}
