using Blazui.Component.Table;
using System;
using System.ComponentModel;

namespace Blazui.Docs.Admin.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        [TableColumn(Text = "Nuget 包名")]
        public string NugetPackageName { get; set; }
        [TableColumn(Text = "简介")]
        public string Description { get; set; }
        [TableColumn(Text = "Github 地址")]
        public string GitHub { get; set; }

        [TableColumn(Text = "最新版本")]
        public string Version { get; set; }

        [TableColumn(Text = "最新版本发布时间")]
        public DateTime? PublishDate { get; set; }
        [TableColumn(Ignore = true)]
        public string ChangeLog { get; set; }
        [TableColumn(Ignore = true)]
        public string Introduction { get; set; }
    }
}
