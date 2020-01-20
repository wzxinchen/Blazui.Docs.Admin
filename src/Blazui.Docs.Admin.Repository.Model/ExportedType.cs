using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ExportedType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Description { get; set; }
        public bool IsGeneric { get; set; }
        public virtual ICollection<ExportedTypeGenericParameter> GenericParameters { get; set; }
        public virtual ICollection<ExportedProperty> Properties { get; set; }
    }
}
