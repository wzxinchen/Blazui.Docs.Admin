using BlazAdmin;
using Blazui.Component;
using Blazui.Component.EventArgs;
using Blazui.Component;
using Blazui.Component.Select;
using Blazui.Component;
using Blazui.Docs.Admin.Enum;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Repository.Model;
using Blazui.Docs.Admin.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Pages.QuickStart
{
    public class QuickStartsBase : BAdminPageBase
    {
        internal BTable table;
        internal BForm form;
        internal List<ProductModel> Products;
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
                Steps.Clear();
                table.MarkAsRequireRender();
                RequireRender = true;
            }
        }
        private int productId;
        private List<ProductVersion> productVersions;
        internal IDictionary<int, string> Versions = new Dictionary<int, string>();
        protected List<QuickStartStepModel> Steps = new List<QuickStartStepModel>();

        [Inject]
        private ProductService ProductService { get; set; }
        protected bool CanUpdate { get; private set; }
        protected bool CanCreate { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            CanUpdate = IsCanAccessAny(DocsResource.UpdateProductQuickStart.ToString());
            CanCreate = IsCanAccessAny(DocsResource.CreateProductQuickStart.ToString());
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Products = ProductService.GetProducts();
        }

        protected void ProductSelected(BChangeEventArgs<BSelectOptionBase<int?>> arg)
        {
            Steps.Clear();
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
                productId = 0;
                productVersions.Clear();
                Versions.Clear();
            }
        }

        protected void Query()
        {
            if (!form.IsValid())
            {
                return;
            }
            Steps = ProductService.GetProductQuickStartSteps(SelectedVersionId.Value).Select(x => new QuickStartStepModel()
            {
                Description = x.Description,
                Title = x.Title,
                Id = x.Id
            }).ToList();
            table.MarkAsRequireRender();
            RequireRender = true;
        }

        protected async Task CreateAsync()
        {
            if (!form.IsValid())
            {
                return;
            }
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(QuickStartStepEdit.ProductId), productId);
            parameters.Add(nameof(QuickStartStepEdit.ProductVersionId), SelectedVersionId.Value);
            await DialogService.ShowDialogAsync<QuickStartStepEdit>("创建入门步骤", 1000, parameters);
            Query();
        }


        protected async Task EditAsync(object arg)
        {
            var stepModel = arg as QuickStartStepModel;
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(QuickStartStepEdit.ProductId), productId);
            parameters.Add(nameof(QuickStartStepEdit.ProductVersionId), SelectedVersionId.Value);
            parameters.Add(nameof(QuickStartStepEdit.Step), new UpdateQuickStartStepModel()
            {
                Content = stepModel.Description,
                Id = stepModel.Id,
                Title = stepModel.Title
            });
            await DialogService.ShowDialogAsync<QuickStartStepEdit>("更新入门步骤", 1000, parameters);
            Query();
        }
        protected async Task MoveUpAsync(object arg)
        {
            var stepModel = arg as QuickStartStepModel;
            await ProductService.StepMoveUpAsync(productId, SelectedVersionId.Value, stepModel.Id);
            Query();
        }

        protected async Task MoveDownAsync(object arg)
        {
            var stepModel = arg as QuickStartStepModel;
            await ProductService.StepMoveDownAsync(productId, SelectedVersionId.Value, stepModel.Id);
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
    }
}
