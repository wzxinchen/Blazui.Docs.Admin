using BlazAdmin;
using Blazui.Component;
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
    public class ChangeLogBase : BAdminPageBase
    {
        protected bool CanUpdate;

        [Inject]
        private ProductService ProductService { get; set; }

        [Parameter]
        public ProductModel Product { get; set; }

        [Parameter]
        public DialogOption Dialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            CanUpdate = IsCanAccessAny(DocsResource.PublishProduct.ToString());
        }

        protected async Task SaveChangeLogAsync()
        {
            await ProductService.UpdateChangeLogAsync(Product.Id, Product.Version, Product.ChangeLog);
            await Dialog.CloseDialogAsync();
        }
    }
}
