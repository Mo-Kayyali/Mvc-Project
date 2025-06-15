using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        void Add(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> GetAll(bool withTracking = false);

        //IQueryable<TEntity> GetAllQueryable();

        IEnumerable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity,bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        TEntity? GetById(int id);
        void Update(TEntity entity);
    }
}
