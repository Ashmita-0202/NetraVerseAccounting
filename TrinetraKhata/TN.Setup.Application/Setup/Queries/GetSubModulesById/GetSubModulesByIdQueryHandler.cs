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

namespace TN.Setup.Application.Setup.Queries.GetSubModulesById
{
    public record GetSubModulesByIdQueryHandler:IRequestHandler<GetSubModulesByIdQuery,Result<GetSubModulesByIdResponse>>
    {
        private readonly ISubModules _subModules;
        private readonly IMapper _mapper;

        public GetSubModulesByIdQueryHandler(ISubModules subModules,IMapper mapper)
        { 
            _subModules=subModules;
            _mapper=mapper;
        }

        public async Task<Result<GetSubModulesByIdResponse>> Handle(GetSubModulesByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subModulesById = await _subModules.GetSubModulesById(request.id);

                return Result<GetSubModulesByIdResponse>.Success(subModulesById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching subModules by Id", ex);
            }
        }
    }

   
}
