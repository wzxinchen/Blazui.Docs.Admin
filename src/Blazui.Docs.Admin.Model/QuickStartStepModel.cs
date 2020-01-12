using Blazui.Component.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Model
{
    public class QuickStartStepModel
    {
        [TableColumn(Ignore = true)]
        public int Id { get; set; }
        [TableColumn(Text = "标题")]
        public string Title { get; set; }
        [TableColumn(Text = "描述")]
        public string Description { get; set; }
    }
}
