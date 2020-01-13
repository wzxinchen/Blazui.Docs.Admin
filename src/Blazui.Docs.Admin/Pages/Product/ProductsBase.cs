using BlazAdmin;
using Blazui.Component;
using Blazui.Component.Table;
using Blazui.Docs.Admin.Enum;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Pages.Product
{
    public class ProductsBase : BAdminPageBase
    {
        internal BTable table;

        [Inject]
        private ProductService productService { get; set; }

        protected bool CanCreate;

        protected bool CanUpdateBasic;
        protected bool CanDelete;

        protected bool CanPublish;
        internal List<ProductModel> products;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            CanCreate = IsCanAccessAny(DocsResource.CreateProduct.ToString());
            CanUpdateBasic = IsCanAccessAny(DocsResource.UpdateProductBasic.ToString());
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
                Id = productModel.Id,
                NugetPackageName = productModel.NugetPackageName
            });
            await DialogService.ShowDialogAsync<ProductEdit>("更新产品基本信息", parameters);
            products = productService.GetProducts();
            table.MarkAsRequireRender();
            RequireRender = true;
        }

        protected async Task PrePublishAsync(object product)
        {
            var productModel = product as ProductModel;
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(ProductPublish.ProductId), productModel.Id);
            await DialogService.ShowDialogAsync<ProductPublish>("发布新版本", 1000, parameters);
            RefreshProducts();
        }

        protected async Task PublishAsync(object product)
        {
            var productModel = product as ProductModel;
            try
            {
                await productService.PublishAsync(productModel.Id);
                RefreshProducts();
            }
            catch (OperationException oe)
            {
                Alert(oe.Message);
            }
        }

        private void RefreshProducts()
        {
            if (table == null)
            {
                return;
            }
            products = productService.GetProducts();
            table.MarkAsRequireRender();
            RequireRender = true;
        }

        protected async Task DelAsync(object product)
        {
            var confirmResult = await ConfirmAsync("这将删除该产品的所有数据，确认删除？");
            if (confirmResult != MessageBoxResult.Ok)
            {
                return;
            }
            var productModel = product as ProductModel;
            await productService.DeleteProductsAsync(productModel.Id);
            RefreshProducts();
        }

        protected async Task EditChangeLogAsync(object product)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(ChangeLog.Product), product);
            await DialogService.ShowDialogAsync<ChangeLog>("更新日志", 800, parameters);
            RefreshProducts();
        }

        protected async Task EditIntroductionAsync(object product)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add(nameof(Introduction.Product), product);
            await DialogService.ShowDialogAsync<Introduction>("更新产品介绍", 800, parameters);
            RefreshProducts();
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
