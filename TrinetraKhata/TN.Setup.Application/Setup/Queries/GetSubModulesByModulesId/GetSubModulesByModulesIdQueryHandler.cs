using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetSubModulesByModulesId
{
    public sealed class GetSubModulesByModulesIdQueryHandler : IRequestHandler<GetSubModulesByModulesIdQuery, Result<List<GetSubModulesByModulesIdResponse>>>
    {
        private readonly ISubModules _subModules;

        public GetSubModulesByModulesIdQueryHandler(ISubModules subModules)
        {
            _subModules = subModules;
            
        }
        public async Task<Result<List<GetSubModulesByModulesIdResponse>>> Handle(GetSubModulesByModulesIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subModulesByModulesId = await _subModules.GetSubModulesByModulesId(request.modulesId,cancellationToken);
                return Result<List<GetSubModulesByModulesIdResponse>>.Success(subModulesByModulesId.Data);


            }catch(Exception ex)
            {
                throw new Exception($"An error occurred while fetching SubModules by {request.modulesId}", ex);
            }
        }
    }
}
