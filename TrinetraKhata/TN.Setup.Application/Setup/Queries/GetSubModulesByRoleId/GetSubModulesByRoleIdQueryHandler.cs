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

namespace TN.Setup.Application.Setup.Queries.GetSubModulesByRoleId
{
    public sealed class GetSubModulesByRoleIdQueryHandler:IRequestHandler<GetSubModulesByRoleIdQuery,Result<List<GetSubModulesByRoleIdResponse>>>
    {
        private readonly ISubModules _subModules;
        private readonly IMapper _mapper;

        public GetSubModulesByRoleIdQueryHandler(ISubModules subModules,IMapper mapper) 
        {
            _subModules=subModules;
            _mapper=mapper;
        }

        public async Task<Result<List<GetSubModulesByRoleIdResponse>>> Handle(GetSubModulesByRoleIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subModulesByRoleId = await _subModules.GetSubModulesByRoleId(request.roleId, cancellationToken);
                return Result<List<GetSubModulesByRoleIdResponse>>.Success(subModulesByRoleId.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sub modules by roleId", ex);
            }
        }
    }
}
