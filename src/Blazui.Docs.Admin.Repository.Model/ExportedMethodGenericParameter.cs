using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ExportedMethodGenericParameter
    {
        public int Id { get; set; }
        public int ExportedMethodId { get; set; }
        public int ExportedTypeId { get; set; }
        public virtual ExportedType ExportedType { get; set; }
    }
}
