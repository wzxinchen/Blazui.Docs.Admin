using BlazAdmin;
using Blazui.Component.EventArgs;
using Blazui.Component.Select;
using Blazui.Docs.Admin.Model;
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
        internal IDictionary<int, string> Versions;

        [Inject]
        private ProductService ProductService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Products = ProductService.GetProducts();
        }

        protected async Task ProductSelectedAsync(BChangeEventArgs<BSelectOptionBase<int>> arg)
        {
            ProductService.GetProductVersionsAsync(arg.NewValue.Value);
            Versions = Products.FirstOrDefault(x => x.Id == arg.NewValue.Value);
        }
    }
}
