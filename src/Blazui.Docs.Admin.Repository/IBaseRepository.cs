using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository
{
    public interface IBaseRepository<T>
        where T : class
    {
        Task CreateAsync(T t);
        T QueryByKey(object key);
        List<T> QueryAll();
        Task<int> DeleteAsync(object key);
    }
}
