using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class Component
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TagName { get; set; }
        public virtual ICollection<ComponentParameter> ComponentParameters { get; set; }
    }
}
