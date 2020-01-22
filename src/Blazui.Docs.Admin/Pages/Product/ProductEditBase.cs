using BlazAdmin;
using Blazui.Component;
using Blazui.Component;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Pages.Product
{
    public class ProductEditBase : BAdminPageBase
    {
        protected BForm form;
        private bool isCreate;

        [Inject]
        private ProductService productService { get; set; }

        [Parameter]
        public CreateProductModel ProductModel { get; set; }

        [Parameter]
        public DialogOption Dialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            isCreate = ProductModel == null;
        }

        protected async Task SubmitAsync()
        {
            if (!form.IsValid())
            {
                return;
            }
            try
            {
                if (isCreate)
                {
                    await productService.CreateProductAsync(form.GetValue<CreateProductModel>());
                }
                else
                {
                    await productService.UpdateProductAsync(form.GetValue<CreateProductModel>());
                }
            }
            catch (OperationException oe)
            {
                Alert(oe.Message);
                return;
            }
            _ = Dialog.CloseDialogAsync();
        }
    }
}
