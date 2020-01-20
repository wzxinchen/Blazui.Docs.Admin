using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ExportedTypeGenericParameter
    {
        public int Id { get; set; }
        public int ExportedTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ExportedType ExportedType { get; set; }
    }
}
