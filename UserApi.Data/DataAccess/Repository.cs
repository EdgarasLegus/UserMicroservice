using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserApi.Domain.Entities;

namespace UserApi.Data.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly UserContext _context;

        public Repository(UserContext context)
        {
            _context = context;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (include != null) query = include(query);
            if (expression != null) query = query.Where(expression);

            return query.AsEnumerable();
        }

        public Task<T> GetFirstOrDefault(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (include != null) query = include(query);
            if (expression != null) query = query.Where(expression);

            return query.FirstOrDefaultAsync();
        }

        public async Task Insert(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }
    }
}
