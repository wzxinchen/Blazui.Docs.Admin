using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ComponentVersion
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public int ProductVersionId { get; set; }

        public virtual Component Component { get; set; }
        public virtual ProductVersion ProductVersion { get; set; }
    }
}
