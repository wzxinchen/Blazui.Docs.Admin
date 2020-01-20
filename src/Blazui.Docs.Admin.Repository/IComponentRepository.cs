using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository
{
    public interface IComponentRepository : IBaseRepository<Model.Component>
    {
        IEnumerable<Model.Component> GetComponentsOfVersion(int versionId);
    }
}
