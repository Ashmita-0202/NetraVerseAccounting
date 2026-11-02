using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.RoleById
{
    public sealed class GetRolesByIdQueryHandler :IRequestHandler<GetRolesByIdQuery,Result<GetRolesByIdResponse>>
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IMapper _mapper;

        public GetRolesByIdQueryHandler(IAuthenticationServices authenticationService, IMapper mapper)
        { 
           _authenticationServices= authenticationService;
            _mapper= mapper;
        
        }

        public async Task<Result<GetRolesByIdResponse>> Handle(GetRolesByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var roleById = await _authenticationServices.GetRolesById(request.Id);

                return Result<GetRolesByIdResponse>.Success(roleById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Roles by Id", ex);

            }
        }
    }
}
