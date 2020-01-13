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
    public class IntroductionBase : BAdminDialogBase
    {
        protected bool CanUpdate;

        [Inject]
        private ProductService ProductService { get; set; }

        [Parameter]
        public ProductModel Product { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            CanUpdate = IsCanAccessAny(DocsResource.UpdateProductBasic.ToString());
        }

        protected async Task SaveIntroductionAsync()
        {
            try
            {
                await ProductService.UpdateIntroductionAsync(Product.Id, Product.Introduction);
                await Dialog.CloseDialogAsync();
            }
            catch (OperationException oe)
            {
                Alert(oe.Message);
            }
        }
    }
}
