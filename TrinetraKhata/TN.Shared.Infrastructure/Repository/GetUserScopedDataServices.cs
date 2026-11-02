using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Static.Roles;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.IRepository;

namespace TN.Shared.Infrastructure.Repository
{
    public class GetUserScopedDataServices : IGetUserScopedData
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public GetUserScopedDataServices(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;

        }

        public async Task<(IQueryable<TEntity> Query, string CompanyId, string InstitutionId, string UserRole, bool IsSuperAdmin)> GetUserScopedData<TEntity>(Expression<Func<TEntity, bool>>? additionalFilter = null) where TEntity : class
        {
            try
            {
                var companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;
                var institutionId = _tokenService.InstitutionId() ?? string.Empty;
                var userRole = _tokenService.GetRole();
                var isSuperAdmin = userRole == Role.SuperAdmin;

                IQueryable<TEntity> query = await _unitOfWork.BaseRepository<TEntity>().GetAllAsyncWithPagination();
        


                if (additionalFilter != null)
                {
                    query = query.Where(additionalFilter);
                }

                return (query, companyId, institutionId, userRole, isSuperAdmin);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
