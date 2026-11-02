using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetModulesByRoleId
{
    public sealed class GetModulesByRoleIdQueryHandler : IRequestHandler<GetModulesByRoleIdQuery, Result<List<GetModulesByRoleIdResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IModule _module;

        public GetModulesByRoleIdQueryHandler(IMapper mapper, IModule module )
        {
            _mapper = mapper;
            _module = module;
            
        }
        public async Task<Result<List<GetModulesByRoleIdResponse>>> Handle(GetModulesByRoleIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var modulesByRoleId = await _module.GetModulesByRoleId(request.roleId, cancellationToken);
                return Result<List<GetModulesByRoleIdResponse>>.Success(modulesByRoleId.Data);

            }catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching modules by roleId", ex);
            }
        }
    }
}
