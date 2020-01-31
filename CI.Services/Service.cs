using CI.Core.Domain.Enumerations;
using CI.Core.Interface;
using CI.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CI.Services
{
    public class Service<T> : IService<T> where T : class
    {
        IUnitOfWork _uow;

        IRepository<T> _repository;

        private bool _disposed;

        public Service(IUnitOfWork uow)
        {
            _uow = uow;
            _repository = _uow.Repository<T>();

        }
         
        public void Add(T entity)
        {
            _repository.Add(entity);
            _uow.Commit();
        }

        public void Add(List<T> entities)
        {
            foreach (T entity in entities) {

                _repository.Add(entity);
            }
           
            _uow.Commit();
        }

        public void Delete(object id)
        {
            var entity = _repository.GetById(id);

            _repository.Remove(entity);

            _uow.Commit();
        }

        public void Delete(List<T> entities)
        {
            foreach (T entity in entities)
            {
                 _repository.Remove(entity);
            }

             _uow.Commit();
        }

        public T FindById(object id)
        {
            var entity = _repository.GetById(id);

            return entity;
        }

        public void Update(T entity)
        {
            _repository.Update(entity);

            _uow.Commit();
        }

        public async Task<T> FindByIdAsync(object id)
        {
          return  await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(T entity)
        {
           await _repository.AddAsync(entity);
           await  _uow.CommitAsync();

           return;
        }

        public async Task AddAsync(List<T> entities)
        {
            foreach (T entity in entities)
            {
                await _repository.AddAsync(entity);
            }

            await  _uow.CommitAsync();

            return;
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);

            await _uow.CommitAsync();

            return;
        }

        public async Task DeleteAsync(object id)
        {
            var entity =await _repository.GetByIdAsync(id);

             await _repository.RemoveAsync(entity);

             await _uow.CommitAsync();

            return;
        }

        public async Task DeleteAsync(List<T> entities)
        {
            foreach (T entity in entities)
            {
                await _repository.RemoveAsync(entity);
            }

            await _uow.CommitAsync();

            return;
        }

        public IQueryable<T> GetQueryable()
        {
            return _repository.AsQueryable();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _repository.GetSingle(predicate);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.GetSingleAsync(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _repository.GetAll(predicate);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, Func<T, bool> orderby, Sorted sorted)
        {
           return _repository.GetAll(predicate, orderby, sorted);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, Func<T, bool> orderby, Sorted sorted, int skip, int take)
        {
            return _repository.GetAll(predicate, orderby, sorted, skip, take);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.GetAllAsync(predicate);
        }

        public async Task<IAsyncEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Func<T, bool> orderby, Sorted sorted)
        {
            return await  _repository.GetAllAsync(predicate, orderby, sorted);
        }

        public async Task<IAsyncEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Func<T, bool> orderby, Sorted sorted, int skip, int take)
        {
            return await _repository.GetAllAsync(predicate, orderby, sorted, skip, take);
        }

        public  Task<int> Count(Expression<Func<T, bool>> predicate)
        {

           var query = _repository.AsQueryable();
           int count= query.Where(predicate).Count();

            return Task.FromResult(count);
        }

        public Task<int> Count()
        {

            var query = _repository.AsQueryable();
            int count = query.Count();

            return Task.FromResult(count);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _uow.Dispose();
                _repository.Dispose();
            }
            _disposed = true;

        }

        ~Service()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

    }
}
