using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository
{
    public interface IBaseRepository<T>
    {
        List<T> QueryAllAsync();
    }
}
