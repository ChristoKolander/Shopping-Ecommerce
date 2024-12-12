using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Shopping.Core.Entities;
using Shopping.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shopping.Infrastructure.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        public ProductContext productcontext { get; }

        public RepositoryBase(ProductContext Productcontext)
        {

           productcontext = Productcontext;
        }


        #region Synchronous Methods

        public IQueryable<T> FindAll()
        {
            return this.productcontext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.productcontext.Set<T>()
                .Where(expression).AsNoTracking();
        }
       

        public void Create(T entity)
        {
          this.productcontext.Set<T>().Add(entity);       
        }
   
        public void Update(T entity)
        {
            this.productcontext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.productcontext.Set<T>().Remove(entity);
        }


        public ICollection<T> FindCollection(Expression<Func<T, bool>> match)
        {
            return productcontext.Set<T>().Where(match).ToList();
        }

        #endregion


        #region Asynchronous Methods

        public async Task<T> GetByIdAsync(int id)
        {
            return await productcontext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            this.productcontext.Set<T>().Add(entity);
            await productcontext.SaveChangesAsync();
            return entity;

        }

        public async Task<T> UpdateAsync(T entity, object key)
        {
            if (entity == null)
                return null;
            T exist = await productcontext.Set<T>().FindAsync(key);
            if (exist != null)
            {
                productcontext.Entry(exist).CurrentValues.SetValues(entity);
                await productcontext.SaveChangesAsync();
            }
            return exist;
        }



        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            
            if (entity !=null)
            {
                productcontext.Set<T>().Remove(entity);
                await productcontext.SaveChangesAsync();
            }
               return;

        }

        public async Task<int> DeleteAsync2(T entity)
        {
            productcontext.Set<T>().Remove(entity);
            return await productcontext.SaveChangesAsync();
        }
        
        public async Task RemoveAsync(T entity)
        {
            productcontext.Set<T>().Remove(entity);
            await productcontext.SaveChangesAsync();
        }
     
        // Prefer using the synchronous method, IQueryable used there.
        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> match)
        {
            return await productcontext.Set<T>().FirstOrDefaultAsync(match);
         
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await productcontext.Set<T>().SingleOrDefaultAsync(match);
        }

        public async Task<ICollection<T>> FindCollectionAsync(Expression<Func<T, bool>> match)
        {
            return await productcontext.Set<T>().Where(match).ToListAsync();
        }

        public IQueryable<T> GetAllIncludingParams(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = FindAll();
            
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public async Task<int> CountAsync()
        {
            return await productcontext.Set<T>().CountAsync();
        }

        #endregion
    }
}