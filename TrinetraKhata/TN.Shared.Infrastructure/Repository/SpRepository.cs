using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;

namespace TN.Shared.Infrastructure.Repository
{
    public class SpRepository : ISpRepository
    {
        private readonly IDbConnection _dbConnection;

        public SpRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("connectionString"));
            
        }
        public async Task<IEnumerable<TEntity>> ExecuteStoredProcedureAsync<TEntity>(string storedProcedure, DynamicParameters dynamicParameters)
        {
            var result = await _dbConnection.QueryAsync<TEntity>(
                storedProcedure,
                dynamicParameters,
                commandType: CommandType.StoredProcedure
                );
            return result;
        }
    }
}
