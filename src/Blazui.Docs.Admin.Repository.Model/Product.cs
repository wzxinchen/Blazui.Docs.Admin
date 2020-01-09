﻿using System;
using System.Collections.Generic;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string NugetPackageName { get; set; }
        public string Description { get; set; }
        public string GitHub { get; set; }
        public string QuickStart { get; set; }
        public virtual ICollection<VersionChange> VersionChanges { get; set; }
    }
}