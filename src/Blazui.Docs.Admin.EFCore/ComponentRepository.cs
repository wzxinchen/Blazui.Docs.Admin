using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository.EFCore
{
    public class ComponentRepository : BaseRepository<Model.Component>, IComponentRepository
    {
        public ComponentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<Model.Component> GetComponentAsync(string tagName)
        {
            return Query.Include(x => x.Versions)
                .ThenInclude(x => x.ProductVersion)
                .Where(x => x.TagName == tagName)
                .FirstOrDefaultAsync();
        }

        public Task<Model.Component> GetComponentAsync(int componentId)
        {
            return Query.Include(x => x.ExportedTypes)
                .Where(x => x.Id == componentId)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<Model.Component> GetComponents(int productVersionId)
        {
            return Query.Include(x => x.Versions)
                .ThenInclude(x => x.ProductVersion)
                .Where(x => x.Versions.Any(y => y.ProductVersionId == productVersionId))
                .ToList();
        }
    }
}
