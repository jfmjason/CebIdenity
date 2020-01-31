
using CI.Core.Domain.Enumerations;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace CI.Core.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> AsQueryable();

        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);

        TEntity GetSingle(Expression<Func<TEntity, Boolean>> predicate);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, Boolean>> predicate);

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, Boolean>> predicate);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, Boolean>> predicate, Func<TEntity, Boolean> orderby, Sorted sorted);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, Boolean>> predicate, Func<TEntity, Boolean> orderby, Sorted sorted, int skip, int take);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, Boolean>> predicate);
        Task<IAsyncEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, Boolean>> predicate, Func<TEntity, Boolean> orderby, Sorted sorted);
        Task<IAsyncEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, Boolean>> predicate, Func<TEntity, Boolean> orderby, Sorted sorted, int skip, int take);

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Remove(object id);
        Task RemoveAsync(object id);
    }
}
