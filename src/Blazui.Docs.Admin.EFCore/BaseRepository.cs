using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin.Repository.EFCore
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public DbContext DbContext { get; }

        public Task CreateAsync(T t)
        {
            DbContext.Set<T>().Add(t);
            return DbContext.SaveChangesAsync();
        }

        public List<T> QueryAll()
        {
            return DbContext.Set<T>().ToList();
        }
    }
}
