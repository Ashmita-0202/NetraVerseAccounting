using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.RoleByUserId
{
    public sealed class GetRolesByUserIdQueryHandler : IRequestHandler<GetRolesByUserIdQuery, Result<List<GetRolesByUserIdQueryResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public GetRolesByUserIdQueryHandler(IUserServices userServices,IMapper mapper) 
        {
            _userServices=userServices;
            _mapper=mapper;
        
        }

        public async Task<Result<List<GetRolesByUserIdQueryResponse>>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roleByUserId = await _userServices.GetRolesByUserId(request.userId, cancellationToken);
                return Result<List<GetRolesByUserIdQueryResponse>>.Success(roleByUserId.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching a roles by userId", ex);

            }
        }
    }
}
