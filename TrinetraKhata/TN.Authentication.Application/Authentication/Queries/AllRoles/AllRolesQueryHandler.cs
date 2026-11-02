using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Authentication.Application.Authentication.Queries.AllRoles
{
    public sealed class AllRolesQueryHandler : IRequestHandler<AllRolesQuery, Result<PagedResult<AllRolesResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public AllRolesQueryHandler(IUserServices userServices, IMapper mapper)
        {
            _userServices = userServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PagedResult<AllRolesResponse>>> Handle(AllRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allroles = await _userServices.GetAllRoles(request.PaginationRequest, cancellationToken);
                var allrolesDisplay = _mapper.Map<PagedResult<AllRolesResponse>>(allroles.Data);

                return Result<PagedResult<AllRolesResponse>>.Success(allrolesDisplay);

            }catch (Exception ex)
            {
                throw new Exception("An error occured while showing roles");
            }
        }
    }
}
