using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.Model
{
    public class QueryStartStep
    {
        public int Id { get; set; }
        public int ProductVersionId { get; set; }
        public int Sort { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
