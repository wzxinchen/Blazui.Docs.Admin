using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class ProductVersion
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public int ProductId { get; set; }

        public DateTime PublishTime { get; set; }

        public virtual ICollection<VersionChange> VersionChanges { get; set; }
    }
}
