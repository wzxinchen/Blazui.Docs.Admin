using Blazui.Docs.Admin.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository
{
    public interface IProductVersionRepository : IBaseRepository<ProductVersion>
    {
        Task<ProductVersion> GetComponentsAsync(int productVersionId);
    }
}
