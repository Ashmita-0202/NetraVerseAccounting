using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class;   
        IDbContextTransaction BeginTransaction();
        Task<int> SaveChangesAsync();
    }
}
