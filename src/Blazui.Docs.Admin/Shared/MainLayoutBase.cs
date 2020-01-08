using BlazAdmin;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        protected List<MenuModel> Menus { get; set; } = new List<MenuModel>();

        protected override void OnInitialized()
        {
            
            Menus.Add(new MenuModel()
            {
                Label = "产品管理",
                Icon = "el-icon-star-on",
                Route = "products"
            });
            Menus.Add(new MenuModel()
            {
                Label = "文档管理",
                Icon = "el-icon-document",
                Route = "docs"
            });
        }
    }
}
