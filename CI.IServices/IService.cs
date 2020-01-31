using CI.Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CI.IServices
{
    public interface IService<T> :IDisposable where T: class
    {
        T FindById(object id);
        void Add(T entity);
        void Add(List<T> entities);

        void Update(T entity);
        void Delete(object id);
        void Delete(List<T> entities);

        Task<T> FindByIdAsync(object id);
        Task AddAsync(T entity);
        Task AddAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        Task DeleteAsync(List<T> entities);

        IQueryable<T> GetQueryable();

        T GetSingle(Expression<Func<T, Boolean>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, Boolean>> predicate);

        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, Boolean>> predicate);
        IEnumerable<T> GetAll(Expression<Func<T, Boolean>> predicate, Func<T, Boolean> orderby, Sorted sorted);
        IEnumerable<T> GetAll(Expression<Func<T, Boolean>> predicate, Func<T, Boolean> orderby, Sorted sorted, int skip, int take);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, Boolean>> predicate);
        Task<IAsyncEnumerable<T>> GetAllAsync(Expression<Func<T, Boolean>> predicate, Func<T, Boolean> orderby, Sorted sorted);
        Task<IAsyncEnumerable<T>> GetAllAsync(Expression<Func<T, Boolean>> predicate, Func<T, Boolean> orderby, Sorted sorted, int skip, int take);

        Task<int> Count();
        Task<int> Count(Expression<Func<T, bool>> predicate);

        

    }
}
