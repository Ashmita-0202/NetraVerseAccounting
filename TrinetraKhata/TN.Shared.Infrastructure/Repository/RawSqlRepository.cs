using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Data;

namespace TN.Shared.Infrastructure.Repository
{
    public class RawSqlRepository : IRawSqlRepository
    {
        private readonly ApplicationDbContext _context;

        public RawSqlRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            
        }
        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            var result = _context.Set<TEntity>().FromSqlRaw(sql, parameters);
            return result;
        }
    }
}
