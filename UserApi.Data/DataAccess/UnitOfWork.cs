using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.Data.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(UserContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var targetType = typeof(T);
            if (!_repositories.ContainsKey(targetType))
            {
                _repositories[targetType] = new Repository<T>(_context);
            }
            return (IRepository<T>)_repositories[targetType];
        }
    }
}
