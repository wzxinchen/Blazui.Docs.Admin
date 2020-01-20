using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazui.Docs.Admin.Repository.EFCore
{
    public class ComponentRepository : BaseRepository<Model.Component>, IComponentRepository
    {
        public ComponentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Model.Component> GetComponentsOfVersion(int versionId)
        {
            return Query.Where(x => x.ProductVersionId == versionId).ToList();
        }
    }
}
