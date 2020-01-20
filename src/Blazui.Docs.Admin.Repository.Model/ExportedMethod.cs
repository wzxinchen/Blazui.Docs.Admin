using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ExportedMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsGeneric { get; set; }

        public virtual ICollection<ExportedMethodGenericParameter> GenericParameters { get; set; }
        public virtual ICollection<ExportedMethodReturnType> ReturnTypes { get; set; }

        public virtual ICollection<ExportedMethodParameter> Parameters { get; set; }
    }
}
