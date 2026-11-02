using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Queries.AllRoles;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Authentication.Application.Authentication.Queries.AllPermission
{
    public class AllPermissionQueryHandler : IRequestHandler<AllPermissionQuery, Result<PagedResult<AllPermissionResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public AllPermissionQueryHandler(IUserServices userServices, IMapper mapper)
        {
            _userServices = userServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<AllPermissionResponse>>> Handle(AllPermissionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allPermission = await _userServices.GetAllPermission(request.PaginationRequest, cancellationToken);
                var allPermissionDisplay = _mapper.Map<PagedResult<AllPermissionResponse>>(allPermission.Data);

                return Result<PagedResult<AllPermissionResponse>>.Success(allPermissionDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while showing roles");
            }
        }
    }
}
