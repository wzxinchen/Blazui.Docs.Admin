using BlazAdmin;
using Blazui.Component;
using Blazui.Component.Form;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Pages
{
    public class ProductEditBase : BAdminPageBase
    {
        protected BForm form;
        [Inject]
        private ProductService productService { get; set; }

        [Parameter]
        public CreateProductModel ProductModel { get; set; }

        [Parameter]
        public DialogOption Dialog { get; set; }

        protected async Task SubmitAsync()
        {
            if (!form.IsValid())
            {
                return;
            }
            try
            {
                await productService.CreateProductAsync(form.GetValue<CreateProductModel>());
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
