using CI.Core.Domain.Common;
using CI.Core.Interface;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace CI.Infra.Data.Imp
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        private bool _disposed;

        private Hashtable _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(TEntity);

            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)_repositories[type];
            }

            IRepository<TEntity> repo = (IRepository<TEntity>)Activator.CreateInstance(typeof(Repository<TEntity>), _context);

            _repositories.Add(type, repo);

            return (IRepository<TEntity>)_repositories[type];

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return ;

            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;

        }

        ~UnitOfWork()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }


    }
}
