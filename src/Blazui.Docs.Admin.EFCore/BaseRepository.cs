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

        public IQueryable<T> Query
        {
            get
            {
                return DbContext.Set<T>();
            }
        }
        public Task CreateAsync(T t)
        {
            DbContext.Set<T>().Add(t);
            return DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(object key)
        {
            var entity = DbContext.Set<T>().Find(key);
            if (entity == null)
            {
                return 0;
            }
            DbContext.Set<T>().Remove(entity);
            return await DbContext.SaveChangesAsync();
        }

        public List<T> QueryAll()
        {
            return DbContext.Set<T>().ToList();
        }

        public T QueryByKey(object key)
        {
            return DbContext.Set<T>().Find(key);
        }
    }
}
