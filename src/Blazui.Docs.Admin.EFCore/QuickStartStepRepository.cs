using Blazui.Docs.Admin.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazui.Docs.Admin.Repository.EFCore
{
    public class QuickStartStepRepository : BaseRepository<QuickStartStep>, IQuickStartStepRepository
    {
        public QuickStartStepRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<QuickStartStep> GetSteps(int productVersionId)
        {
            return Query.Where(x => x.ProductVersionId == productVersionId).OrderBy(x => x.Sort).ToList();
        }
    }
}
