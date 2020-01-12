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

namespace Blazui.Docs.Admin.Pages.Product
{
    public class ProductPublishBase : BAdminPageBase
    {
        protected BForm form;
        [Parameter]
        public DialogOption Dialog { get; set; }

        [Inject]
        private ProductService ProductService { get; set; }

        [Parameter]
        public int ProductId { get; set; }

        protected async Task PublishAsync()
        {
            if (!form.IsValid())
            {
                return;
            }

            var versionModel = form.GetValue<PublishVersionModel>();
            versionModel.ProductId = ProductId;
            await ProductService.PublishAsync(versionModel);
            _ = Dialog.CloseDialogAsync();
        }
    }
}
