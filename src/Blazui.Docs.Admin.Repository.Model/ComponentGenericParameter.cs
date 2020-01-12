using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ComponentGenericParameter
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
