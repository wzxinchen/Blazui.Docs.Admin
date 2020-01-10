using BlazAdmin;
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

        [Inject]
        private ProductService ProductService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Products = ProductService.GetProducts();
        }
    }
}
