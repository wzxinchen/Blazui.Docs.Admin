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

        public bool IsPublish { get; set; }

        public string ChangeLog { get; set; }

        public virtual ICollection<VersionChange> VersionChanges { get; set; }
        public virtual ICollection<QuickStartStep> QueryStartSteps { get; set; } = new List<QuickStartStep>();
        public virtual ICollection<Component> Components { get; set; } = new List<Component>();
    }
}
