using Blazui.Docs.Admin.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository.EFCore
{
    public class ExportedTypeRepository : BaseRepository<ExportedType>, IExportedTypeRepository
    {
        public ExportedTypeRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<ExportedType>> GetExportedTypesAsync(int productVersionId)
        {
            return Query.Where(x => x.ProductVersionId == productVersionId).ToListAsync();
        }
    }
}
