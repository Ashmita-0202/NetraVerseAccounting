using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Queries.AllUsers;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Authentication.Application.Authentication.Queries.AssignableRolesByPermissionId
{
    public sealed class AssignableRolesByPermissionIdQueryHandler : IRequestHandler<AssignableRolesByPermissionIdQuery, Result<List<AssignableRolesByPermissionIdResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public AssignableRolesByPermissionIdQueryHandler(IUserServices userServices, IMapper mapper)
        {
            _mapper = mapper;
            _userServices = userServices;

        }

        public async Task<Result<List<AssignableRolesByPermissionIdResponse>>> Handle(AssignableRolesByPermissionIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var assignableRolesByPermission = await _userServices.AssignableRolesByPermissionId(request.permissionId, cancellationToken);

                var assignableRolesByPermissionDisplay = _mapper.Map<List<AssignableRolesByPermissionIdResponse>>(assignableRolesByPermission.Data);

                return Result<List<AssignableRolesByPermissionIdResponse>>.Success(assignableRolesByPermissionDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while showing Assignable User");
            }
        }
    }
}
