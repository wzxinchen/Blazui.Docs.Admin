using System;
using System.ComponentModel;

namespace Blazui.Docs.Admin.Enum
{
    public enum ChangeType
    {
        [Description("修复")]
        FixIssue,

        [Description("新接口")]
        AddApi,

        [Description("新组件")]
        AddComponent,

        [Description("移除接口")]
        RemoveApi,

        [Description("移除组件")]
        RemoveComponent
    }
}
