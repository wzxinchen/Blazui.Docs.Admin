using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Docs.Admin.Repository.EFCore
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        public List<T> QueryAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
