using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Data;

namespace TN.Shared.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _dbSet  = _context.Set<TEntity>();
            
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRange(List<TEntity> entity)
        {
            await _dbSet.AddRangeAsync(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.AnyAsync(filter);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeleteAllAsync()
        {
            var allData = await _dbSet.ToListAsync();
            _dbSet.RemoveRange(allData);
        }

        public void DeleteRange(List<TEntity> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public async Task<IQueryable<TEntity>> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = _dbSet.Where(predicate);
                return await Task.FromResult(result);

            }catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
            ? await _dbSet.AsQueryable().FirstOrDefaultAsync()
            : await _dbSet.AsQueryable().FirstOrDefaultAsync(predicate);
        }

        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(_dbSet.AsQueryable());
        }

        public Task<IQueryable<TEntity>> GetAllAsyncWithPagination()
        {
           return Task.FromResult(_dbSet.AsNoTracking().AsQueryable());
        }

        public async Task<IEnumerable<TEntity>> GetAllWithIncludeAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            if(predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByGuIdAsync(string guid) => await _dbSet.FindAsync(guid);
        

        public async Task<TEntity> GetById(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<TEntity>> GetConditionalAsync(
            Expression<Func<TEntity, bool>>? predicate=null, Func<IQueryable<TEntity>,
                IQueryable<TEntity>>? queryModifier = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply filter if predicate is provided
            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            // Apply eager loading
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Apply additional query modification
            if (queryModifier is not null)
            {
                query = queryModifier(query);
            }

            return await query.ToListAsync();

        }

        public async Task<IEnumerable<TResult>> GetConditionalFilterType<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TResult>> queryModifier)
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);
            IQueryable<TResult> modifiedQuery = queryModifier(query);
            return await modifiedQuery.ToListAsync();
        }

        public async Task<List<TEntity>> GetFilterAndOrderByAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if(predicate is not null)
            {
                query = query.Where(predicate);
            }

            if(orderby is not null)
            {
                query = orderby(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.SingleOrDefaultAsync(predicate);


        public async Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderBy = null)
        {
            if (orderBy == null)
                throw new ArgumentNullException(nameof(orderBy), "An orderBy expression must be provided for LastOrDefaultAsync.");

            var query = _dbSet.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);


            return await query.OrderByDescending(orderBy).FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task UpdateRange(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            await Task.CompletedTask; // To maintain async pattern consistency
        }
    }
}
