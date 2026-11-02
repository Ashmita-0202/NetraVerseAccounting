using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.UserByRoleId
{
   public class GetUserByRoleIdQueryHandler :IRequestHandler<GetUserByRoleIdQuery, Result<List<GetUserByRoleIdQueryResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public GetUserByRoleIdQueryHandler(IUserServices userServices, IMapper mapper)
        {
            _userServices=userServices;
            _mapper=mapper;
        }

        public async Task<Result<List<GetUserByRoleIdQueryResponse>>> Handle(GetUserByRoleIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var usersByRoleId = await _userServices.GetUserByRoleId(request.roleId, cancellationToken);
                    return Result<List<GetUserByRoleIdQueryResponse>>.Success(usersByRoleId.Data);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching users by Role ID {request.roleId}", ex);
            }
        }
    }
}
