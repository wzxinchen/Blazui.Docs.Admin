using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository
{
    public interface IComponentRepository : IBaseRepository<Model.Component>
    {
        IEnumerable<Model.Component> GetComponents(int productVerionsId);
        Task<Model.Component> GetComponentAsync(string tagName);
        Task<Model.Component> GetComponentAsync(int componentId);
    }
}
