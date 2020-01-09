using Blazui.Docs.Admin.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class VersionChange
    {
        public int Id { get; set; }
        public int ProductVersionId { get; set; }
        public ChangeType Type { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
