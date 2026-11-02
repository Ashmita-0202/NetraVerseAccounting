using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.IRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        
        Task<TEntity> GetByGuIdAsync(string guid); 
        Task<TEntity> GetById(int id);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetConditionalAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null,
            params Expression<Func<TEntity, object>>[] includes
            );


        

        Task<IEnumerable<TResult>> GetConditionalFilterType<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TResult>> queryModifier);


        Task<IEnumerable<TEntity>> GetAllWithIncludeAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> GetAllAsyncWithPagination();
        Task<List<TEntity>> GetFilterAndOrderByAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderBy = null);



        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        Task AddRange(List<TEntity> entity);
        void DeleteRange(List<TEntity> entity);
        Task DeleteAllAsync();
        Task UpdateRange(List<TEntity> entities);
    }
}
