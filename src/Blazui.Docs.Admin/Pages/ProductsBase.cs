using BlazAdmin;
using Blazui.Component.Table;
using Blazui.Docs.Admin.Enum;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Pages
{
    public class ProductsBase : BAdminPageBase
    {
        internal BTable table;
        [Inject]
        private ProductService productService { get; set; }

        protected bool CanCreate;

        protected bool CanUpdateBasic;

        protected bool CanUpdateQuickStart;
        protected bool CanDelete;

        protected bool CanPublish;
        internal List<ProductModel> products;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            CanCreate = IsCanAccessAny(DocsResource.CreateProduct.ToString());
            CanUpdateBasic = IsCanAccessAny(DocsResource.UpdateProductBasic.ToString());
            CanUpdateQuickStart = IsCanAccessAny(DocsResource.UpdateProductQuickStart.ToString());
            CanDelete = IsCanAccessAny(DocsResource.DeleteProduct.ToString());
            CanPublish = IsCanAccessAny(DocsResource.PublishProduct.ToString());
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (products == null)
            {
                products = productService.GetProducts();
            }
        }

        protected async Task EditAsync(object product)
        {
            var productModel = product as ProductModel;
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(ProductEdit.ProductModel), new CreateProductModel()
            {
                Description = productModel.Description,
                GitHub = productModel.GitHub,
                NugetPackageName = productModel.NugetPackageName
            });
            await DialogService.ShowDialogAsync<ProductEdit>("更新产品基本信息", parameters);
            products = productService.GetProducts();
            table.MarkAsRequireRender();
            RequireRender = true;
        }

        protected async Task EditQuickStartAsync(object product)
        {
            var productModel = product as ProductModel;
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(ProductEdit.ProductModel), new CreateProductModel()
            {
                Description = productModel.Description,
                GitHub = productModel.GitHub,
                NugetPackageName = productModel.NugetPackageName
            });
            await DialogService.ShowDialogAsync<ProductEdit>("更新产品基本信息", parameters);
            products = productService.GetProducts();
            table.MarkAsRequireRender();
            RequireRender = true;
        }
        protected async Task PublishAsync(object product)
        {
            var productModel = product as ProductModel;
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(ProductEdit.ProductModel), new CreateProductModel()
            {
                Description = productModel.Description,
                GitHub = productModel.GitHub,
                NugetPackageName = productModel.NugetPackageName
            });
            await DialogService.ShowDialogAsync<ProductEdit>("更新产品基本信息", parameters);
            products = productService.GetProducts();
            table.MarkAsRequireRender();
            RequireRender = true;
        }
        protected async Task DelAsync(object product)
        {
            var productModel = product as ProductModel;
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(ProductEdit.ProductModel), new CreateProductModel()
            {
                Description = productModel.Description,
                GitHub = productModel.GitHub,
                NugetPackageName = productModel.NugetPackageName
            });
            await DialogService.ShowDialogAsync<ProductEdit>("更新产品基本信息", parameters);
            products = productService.GetProducts();
            table.MarkAsRequireRender();
            RequireRender = true;
        }
        protected async Task CreateAsync()
        {
            await DialogService.ShowDialogAsync<ProductEdit>("创建产品基本信息");
            products = productService.GetProducts();
            table.MarkAsRequireRender();
            RequireRender = true;
        }
    }
}
