using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ExportedProperty
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string PropertyType { get; set; }
        public bool IsComponentParameter { get; set; }
        public bool IsComponentCascadingParameter { get; set; }
        public string Description { get; set; }
        public int ExportedTypeId { get; set; }

        public virtual ExportedType ExportedType { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
