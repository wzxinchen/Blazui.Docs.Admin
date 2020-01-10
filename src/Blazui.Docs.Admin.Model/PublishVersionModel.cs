using Blazui.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Model
{
    public class PublishVersionModel
    {
        public string ChangeLog { get; set; }
        public IFileModel FilePackage { get; set; }
    }
}
