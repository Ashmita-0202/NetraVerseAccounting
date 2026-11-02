using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.ServiceInterface
{
    public interface IGetUserScopedData
    {
        Task<(IQueryable<TEntity> Query, string CompanyId, string InstitutionId, string UserRole, bool IsSuperAdmin)>
        GetUserScopedData<TEntity>(Expression<Func<TEntity, bool>>? additionalFilter = null) where TEntity : class;

    }
}
