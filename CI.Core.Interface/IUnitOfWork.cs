using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CI.Core.Interface
{
    public interface IUnitOfWork : IDisposable
    {

        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        int Commit();

        Task<int> CommitAsync();
        Task<int> CommitAsync(CancellationToken cancellationToken);

        void Dispose(bool disposing);

    }
}
