using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string NugetPackageName { get; set; }
        public string Description { get; set; }
        public string GitHub { get; set; }
        public virtual ICollection<VersionChange> VersionChanges { get; set; }
        public virtual ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
