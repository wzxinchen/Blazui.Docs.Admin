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
        public int ProductVersionId { get; set; }
        public int? ComponentId { get; set; }
        public virtual ICollection<ExportedTypeGenericParameter> GenericParameters { get; set; } = new List<ExportedTypeGenericParameter>();
        public virtual ICollection<ExportedProperty> Properties { get; set; } = new List<ExportedProperty>();
        public virtual Component Component { get; set; }

        public override string ToString()
        {
            return $"{Namespace}.{Name}";
        }
    }
}
