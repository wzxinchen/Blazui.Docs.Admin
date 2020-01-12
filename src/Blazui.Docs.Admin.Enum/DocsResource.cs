using BlazAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Blazui.Docs.Admin.Enum
{
    [Resources]
    public enum DocsResource
    {
        [Description("新增产品")]
        CreateProduct,

        [Description("更新产品基础信息")]
        UpdateProductBasic,

        [Description("更新产品入门信息")]
        UpdateProductQuickStart,

        [Description("创建产品入门信息")]
        CreateProductQuickStart,

        [Description("删除产品")]
        DeleteProduct,

        [Description("发布产品")]
        PublishProduct
    }
}
