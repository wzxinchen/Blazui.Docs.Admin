using Blazui.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Model
{
    public class PublishVersionModel
    {
        public int ProductId { get; set; }
        public string ChangeLog { get; set; }
        public IFileModel FilePackage { get; set; }
        public string Version { get; set; }
    }
}
