using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.Data.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
        IRepository<T> GetRepository<T>() where T : class;
    }
}
