using Microsoft.EntityFrameworkCore;
using Shopping.Core.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Shopping.Infrastructure.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        public ProductContext productcontext { get; }

        public RepositoryBase(ProductContext Productcontext)
        {

           productcontext = Productcontext;
        }

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

        public IQueryable<T> FindAllAsync()
        {
            return this.productcontext.Set<T>().AsNoTracking();
        }

       
    }
}