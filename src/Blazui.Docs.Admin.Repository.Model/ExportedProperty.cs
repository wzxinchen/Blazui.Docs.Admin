using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ExportedProperty
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int PropertyTypeId { get; set; }
        public bool IsComponentParameter { get; set; }
        public bool IsComponentCascadingParameter { get; set; }
        public ExportedType PropertyType { get; set; }
        public string Description { get; set; }
    }
}
