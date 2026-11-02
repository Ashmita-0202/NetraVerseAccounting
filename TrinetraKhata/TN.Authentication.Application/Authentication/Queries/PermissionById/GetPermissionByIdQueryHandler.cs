using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Authentication.Application.Authentication.Queries.RoleById;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.PermissionById
{
    public sealed class GetPermissionByIdQueryHandler:IRequestHandler<GetPermissionByIdQuery, Result<GetPermissionByIdQueryResponse>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public GetPermissionByIdQueryHandler(IUserServices userServices,IMapper mapper) 
        {
            _userServices = userServices;
            _mapper=mapper;
        }

        public async Task<Result<GetPermissionByIdQueryResponse>> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var permissionById = await _userServices.GetPermissionById(request.id);

                return Result<GetPermissionByIdQueryResponse>.Success(permissionById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Permission by Id", ex);

            }
        }
    }
}
