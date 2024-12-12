using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Shopping.Core.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        ICollection<T> FindCollection(Expression<Func<T, bool>> match);

        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity, object key);

        Task DeleteAsync(int id);
        Task RemoveAsync(T entity);
        Task<int> DeleteAsync2(T entity);

        Task<T> FindByConditionAsync(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindCollectionAsync(Expression<Func<T, bool>> match);

        IQueryable<T> GetAllIncludingParams(params Expression<Func<T, object>>[] includeProperties);
        Task<int> CountAsync();

    }
}
