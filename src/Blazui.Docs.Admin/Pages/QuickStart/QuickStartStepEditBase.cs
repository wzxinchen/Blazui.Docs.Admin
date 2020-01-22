using BlazAdmin;
using Blazui.Component;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Pages.QuickStart
{
    public class QuickStartStepEditBase : BAdminDialogBase
    {
        protected BForm form;
        private bool isCreate;

        [Inject]
        private ProductService ProductService { get; set; }

        [Parameter]
        public UpdateQuickStartStepModel Step { get; set; }

        [Parameter]
        public int ProductVersionId { get; set; }

        [Parameter]
        public int ProductId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            isCreate = Step == null;
        }

        protected async Task SubmitAsync()
        {
            if (!form.IsValid())
            {
                return;
            }

            var updateModel = form.GetValue<UpdateQuickStartStepModel>();
            if (isCreate)
            {
                await ProductService.CreateQuickStartStepAsync(ProductId, ProductVersionId, updateModel);
            }
            else
            {
                await ProductService.UpdateQuickStartStepAsync(updateModel);
            }
            _ = Dialog.CloseDialogAsync();
        }
    }
}
