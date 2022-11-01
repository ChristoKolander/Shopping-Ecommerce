using Shopping.Core.Entities.RequestFeatures;
using Shopping.Core.Entities;
using Shopping.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Shopping.Infrastructure.Data.GenericRepositoriesRemake
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<bool> Delete(TEntity entityToDelete);
        Task<bool> Delete(object id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetByID(object id);
        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entityToUpdate);

    }

 }
