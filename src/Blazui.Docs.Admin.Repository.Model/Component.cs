using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class Component
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ExportedType> ExportedTypes { get; set; } = new List<ExportedType>();
        public virtual ICollection<ComponentVersion> Versions { get; set; } = new List<ComponentVersion>();
    }
}
