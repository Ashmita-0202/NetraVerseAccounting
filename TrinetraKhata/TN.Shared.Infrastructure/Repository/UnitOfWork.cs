using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Data;

namespace TN.Shared.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;
        private Hashtable _repositories;
        public UnitOfWork(ApplicationDbContext applicationDbContext) { 

            _context = applicationDbContext;
        }
        public IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class
        {
            if(_repositories is null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if(!_repositories.ContainsKey(type))
            {
                var repositoriesType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoriesType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepository<TEntity>)_repositories[type];
        }

        public IDbContextTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed  = true;
            }
           
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
