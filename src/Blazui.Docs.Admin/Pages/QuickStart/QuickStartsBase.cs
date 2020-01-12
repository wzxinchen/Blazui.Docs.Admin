using BlazAdmin;
using Blazui.Component.EventArgs;
using Blazui.Component.Select;
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
        internal List<ProductModel> Products;
        internal int SelectedVersionId;
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

        protected void ProductSelected(BChangeEventArgs<BSelectOptionBase<int>> arg)
        {
            productVersions = ProductService.GetProductVersions(arg.NewValue.Value);
            Versions = productVersions.ToDictionary(x => x.Id, x => x.Version);
        }

        protected void Query()
        {
            var productVersion = productVersions.FirstOrDefault(x => x.Id == SelectedVersionId);
            Steps = ProductService.GetProductQuickStartSteps(productVersion.Id).Select(x => new QuickStartStepModel()
            {
                Description = x.Description,
                Title = x.Title,
                Id = x.Id
            }).ToList();
        }

        protected async Task CreateAsync()
        {

        }
    }
}
