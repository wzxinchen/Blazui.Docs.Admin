using BlazAdmin;
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
        [Inject]
        private ProductService productService { get; set; }
        internal List<ProductModel> products;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (products == null)
            {
                productService.GetProductsAsync();
            }
        }
    }
}
