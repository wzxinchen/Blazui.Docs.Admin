using Blazui.Docs.Admin.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository.EFCore
{
    public class ProductVersionRepository : BaseRepository<ProductVersion>, IProductVersionRepository
    {
        public ProductVersionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<ProductVersion> GetComponentsAsync(int productVersionId)
        {
            return Query.Include(x => x.Components)
                .FirstOrDefaultAsync(x => x.Id == productVersionId);
        }
    }
}
