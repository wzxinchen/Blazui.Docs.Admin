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
    public class ProductPublisbBase : BAdminPageBase
    {
        protected BForm form;
        [Parameter]
        public DialogOption Dialog { get; set; }

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
            await ProductService.PublishAsync(versionModel);
        }
    }
}
