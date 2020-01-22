using Blazui.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Model
{
    public class AdminComponentModel
    {
        [TableColumn(Ignore = true)]
        public int Id { get; set; }
        [TableColumn(Text = "名称")]
        public string Name { get; set; }
        [TableColumn(Text = "调用标签")]
        public string TagName { get; set; }
        [TableColumn(Text = "简介")]
        public string Description { get; set; }
        [TableColumn(Text = "添加于")]
        public string AddAtVersion { get; set; }
    }
}
