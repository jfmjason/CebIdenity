using CI.Core.Domain.Enumerations;
using CI.Core.Domain.Models;
using CI.Core.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CI.Infra.Data.Imp
{
   public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private readonly DbContext _context;

        private bool _disposed;

        public  Repository(DbContext context)
        {
            _context = context;

        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var query = _context.Set<TEntity>();

            return query;
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _context.AddAsync(entity);

            return;
        }

        public TEntity GetById(object id)
        {
            var dbset = _context.Set<TEntity>();

            return dbset.Find(id);
        }

        public async Task<TEntity>GetByIdAsync(object id)
        {
            var dbset = _context.Set<TEntity>();

            return await dbset.FindAsync(id);
        }

        public void Remove(object id)
        {
            var entity = GetById(id);

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Remove(entity);
        }

        public async Task RemoveAsync(object id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Remove(entity);

            return;
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Update(entity);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, Boolean>> predicate)
        {
            var query = _context.Set<TEntity>().Where(predicate);

            return query;
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, Boolean>> predicate, Func<TEntity, Boolean> orderby, Sorted sorted)
        {
            var context = _context.Set<TEntity>();

            var query = sorted == Sorted.DESCENDING ? context.Where(predicate).OrderByDescending(orderby) : context.Where(predicate).OrderBy(orderby);

            return query;
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, Boolean>> predicate, Func<TEntity, Boolean> orderby, Sorted sorted, int skip, int take)
        {
            var context = _context.Set<TEntity>();

            var query = sorted == Sorted.DESCENDING ? context.Where(predicate).OrderByDescending(orderby) : context.Where(predicate).OrderBy(orderby);

            return query.Skip(skip).Take(take);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var query = _context.Set<TEntity>();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, Boolean>>  predicate)
        {
            var query = _context.Set<TEntity>().Where(predicate);

            return await query.ToListAsync();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _context.Set<TEntity>();

            return query.FirstOrDefault(predicate);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _context.Set<TEntity>();
      
            return await query.FirstOrDefaultAsync(predicate);
        }

        public Task<IAsyncEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, bool> orderby, Sorted sorted)
        {
            var context = _context.Set<TEntity>();

            var query = sorted == Sorted.DESCENDING ? context.Where(predicate).OrderByDescending(orderby) : context.Where(predicate).OrderBy(orderby);

            return  Task.FromResult(query.ToAsyncEnumerable());
        }

        public Task<IAsyncEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, bool> orderby, Sorted sorted, int skip, int take)
        {
            var context = _context.Set<TEntity>();

            var query = sorted == Sorted.DESCENDING ? 
                        context.Where(predicate).OrderByDescending(orderby).Skip(skip).Take(take)
                       : context.Where(predicate).OrderBy(orderby).Skip(skip).Take(take);

            return Task.FromResult(query.ToAsyncEnumerable());
        }


    }
}
