using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.District;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.SubModules
{
    public sealed class GetAllSubModulesQueryHandler : IRequestHandler<GetAllSubModulesQuery, Result<PagedResult<GetAllSubModulesResponse>>>
    {
        private readonly ISubModules _subModules;
        private readonly IMapper _mapper;

        public GetAllSubModulesQueryHandler(ISubModules subModules,IMapper mapper)
        {
            _subModules = subModules;
            _mapper = mapper;
             
        }
        public async Task<Result<PagedResult<GetAllSubModulesResponse>>> Handle(GetAllSubModulesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSubModules = await _subModules.GetAllSubModules(request.PaginationRequest, cancellationToken);
                var allSubModulesDisplay = _mapper.Map<PagedResult<GetAllSubModulesResponse>>(allSubModules.Data);

                return Result<PagedResult<GetAllSubModulesResponse>>.Success(allSubModulesDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all SubModules", ex);
            }
        }
    }
}
