using Blazui.Docs.Admin.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository
{
    public interface IQuickStartStepRepository : IBaseRepository<QuickStartStep>
    {
        List<QuickStartStep> GetSteps(int productVersionId);
    }
}
