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

namespace Blazui.Docs.Admin.Pages.ComponentDocs
{
    public class ComponentEditBase : BAdminDialogBase
    {
        protected BTransfer transfer;
        protected BForm form;
        [Parameter]
        public int ProductId { get; set; }

        [Parameter]
        public int ProductVersionId { get; set; }

        [Parameter]
        public int ComponentId { get; set; }
        [Parameter]
        public ComponentEditModel Component { get; set; }

        [Inject]
        private ComponentService ComponentService { get; set; }
        public bool IsCreate { get; private set; }
        protected bool CanCreate { get; set; }
        protected bool CanUpdate { get; set; }

        protected string ButtonText { get; set; }
        protected bool ButtonDisabled { get; set; }
        public List<TransferItem> AllExportedTypes { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            IsCreate = Component == null;

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
            {
                return;
            }
            transfer.FormItem.OriginValueHasRendered = false;
            CanCreate = IsCanAccessAny(DocsResource.CreateComponent.ToString());
            CanUpdate = IsCanAccessAny(DocsResource.UpdateComponent.ToString());
            if (IsCreate && CanCreate)
            {
                ButtonText = "创建";
                ButtonDisabled = false;
            }
            else if (IsCreate && !CanCreate)
            {
                ButtonText = "无权限创建";
                ButtonDisabled = true;
            }
            else if (!IsCreate && CanUpdate)
            {
                ButtonText = "保存";
                ButtonDisabled = false;
            }
            else if (!IsCreate && !CanUpdate)
            {
                ButtonText = "无权限保存";
                ButtonDisabled = true;
            }

            AllExportedTypes = (await ComponentService.GetExportedTypesAsync(ProductVersionId)).Select(x => new TransferItem()
            {
                Id = x.Id.ToString(),
                Label = x.Name
            }).ToList();
            RequireRender = true;
            StateHasChanged();
        }

        protected async Task SubmitAsync()
        {
            if (!form.IsValid())
            {
                return;
            }

            var component = form.GetValue<ComponentEditModel>();
            try
            {
                if (IsCreate)
                {
                    await ComponentService.CreateComponentAsync(ProductVersionId, component);
                }
                else
                {
                    await ComponentService.UpdateComponentAsync(ProductVersionId, ComponentId, component);
                }
                _ = CloseAsync();
            }
            catch (OperationException oe)
            {
                Toast(oe.Message);
            }
        }
    }
}
