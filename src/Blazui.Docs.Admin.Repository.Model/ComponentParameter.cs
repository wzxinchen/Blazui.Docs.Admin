using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ComponentParameter
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public string PropertyName { get; set; }
        public string TypeName { get; set; }

        public bool IsCascading { get; set; }
        public string Description { get; set; }
    }
}
