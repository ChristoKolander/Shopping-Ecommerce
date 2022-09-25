using Microsoft.EntityFrameworkCore;
using Shopping.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shopping.Api.Repositories.Interfaces
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        public ShoppingDbContext ShoppingDbcontext { get; }

        public RepositoryBase(ShoppingDbContext shoppingDbcontext)
        {

            ShoppingDbcontext = shoppingDbcontext;
        }

        public IQueryable<T> FindAll()
        {
            return this.ShoppingDbcontext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.ShoppingDbcontext.Set<T>()
                .Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
          this.ShoppingDbcontext.Set<T>().Add(entity);       
        }

        public void Update(T entity)
        {
            this.ShoppingDbcontext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.ShoppingDbcontext.Set<T>().Remove(entity);
        }
    }
}