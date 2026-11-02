using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.IRepository
{
    public interface ISpRepository
    {
        Task<IEnumerable<TEntity>> ExecuteStoredProcedureAsync<TEntity>(string storedProcedure, DynamicParameters dynamicParameters);
    }
}
