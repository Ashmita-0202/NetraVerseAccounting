using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.GetModulesByRoleId;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.MenuByRoleId
{
   public sealed class  GetMenuByRoleIdQueryHandler:IRequestHandler<GetMenuByRoleIdQuery,Result<List<GetMenuByRoleIdResponse>>>
    {
        private readonly IMenuServices _menuServices;
        private readonly IMapper _mapper;

        public GetMenuByRoleIdQueryHandler(IMenuServices menuServices,IMapper mapper)
        {
            _menuServices=menuServices;
            _mapper=mapper;

        }

        public async Task<Result<List<GetMenuByRoleIdResponse>>> Handle(GetMenuByRoleIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var menuByRoleId = await _menuServices.GetMenuByRoleId(request.roleId, cancellationToken);
                return Result<List<GetMenuByRoleIdResponse>>.Success(menuByRoleId.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching modules by roleId", ex);
            }
        }
    }
}
