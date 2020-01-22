using Blazui.Docs.Admin.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository
{
    public interface IExportedTypeRepository : IBaseRepository<ExportedType>
    {
        Task<List<ExportedType>> GetExportedTypesAsync(int productVersionId);
    }
}
