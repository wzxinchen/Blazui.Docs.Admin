using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ExportedMethodParameter
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ExportedMethodId { get; set; }
        public int ExportedTypeId { get; set; }
        public ExportedType ExportedType { get; set; }
    }
}
