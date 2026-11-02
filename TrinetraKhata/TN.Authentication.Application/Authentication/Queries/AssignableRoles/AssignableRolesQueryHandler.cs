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

namespace TN.Authentication.Application.Authentication.Queries.AssignableRoles
{
    public sealed class AssignableRolesQueryHandler : IRequestHandler<AssignableRolesQuery, Result<List<AssignableRolesResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public AssignableRolesQueryHandler(IUserServices userServices, IMapper mapper)
        {
            _userServices = userServices;
            _mapper = mapper;

        }
        public async Task<Result<List<AssignableRolesResponse>>> Handle(AssignableRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var assignableRoles = await _userServices.AssignableRoles();
                var assignableRolesDisplay = _mapper.Map<List<AssignableRolesResponse>>(assignableRoles.Data);

                return Result<List<AssignableRolesResponse>>.Success(assignableRolesDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while displaying assignable roles");
            }
        }
    }
}
