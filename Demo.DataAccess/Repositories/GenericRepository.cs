using Demo.DataAccess.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Repositories
{
    public class GenericRepository<TEntity>(ApplicationDbContext context)
        : IGenericRepository<TEntity>
        where TEntity : BaseEntity
    {
        //ApplicationDbContext context = new ApplicationDbContext();


        protected readonly ApplicationDbContext _context = context;

        //GET
        public TEntity? GetById(int id) => _context.Set<TEntity>().Find(id);
        //GET ALL
        public IEnumerable<TEntity> GetAll(bool withTracking = false)
            => withTracking ? _context.Set<TEntity>().Where(D => !D.IsDeleted).ToList() :
            _context.Set<TEntity>().AsNoTracking().Where(D => !D.IsDeleted).ToList();
        //ADD
        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            
        }
        //UPDATE

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            
        }
        //DELETE

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var include in includes)
            {
                query.Include(include);
            }
            return query.AsNoTracking().Where(predicate).Select(selector).ToList();
        }

        


        //public IQueryable<TEntity> GetAllQueryable() =>
        //    _context.Set<TEntity>().AsNoTracking().Where(D => !D.IsDeleted);
    }
}
