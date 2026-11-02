using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.ModulesById
{
    public class GetModulesByIdQueryHandler : IRequestHandler<GetModulesByIdQuery, Result<GetModulesByIdResponse>>
    {
        private readonly IModule _module;
        private readonly IMapper _mapper;

        public GetModulesByIdQueryHandler(IModule module, IMapper mapper)
        {
        
            _module=module;
            _mapper=mapper;
        
        
        }

        public async Task<Result<GetModulesByIdResponse>> Handle(GetModulesByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var modulesById = await _module.GetModulesById(request.id);

                return Result<GetModulesByIdResponse>.Success(modulesById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching modules by Id", ex);
            }

        }
    }
}
