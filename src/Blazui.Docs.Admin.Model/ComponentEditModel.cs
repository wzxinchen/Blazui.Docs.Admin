using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Model
{
    public class ComponentEditModel
    {
        public string Name { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public List<string> ExportedTypeIds { get; set; }
    }
}
