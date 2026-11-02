using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Setup.Application.Setup.Queries.Institution;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.Modules
{
    public sealed class GetAllModulesQueryHandler : IRequestHandler<GetAllModulesQuery, Result<PagedResult<GetAllModulesResponse>>>
    {
        private readonly IModule _module;
        private readonly IMapper _mapper;

        public GetAllModulesQueryHandler(IModule module,IMapper mapper) 
        {
            _module=module; 
            _mapper=mapper;
        }

       

        public async Task<Result<PagedResult<GetAllModulesResponse>>> Handle(GetAllModulesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allModule = await _module.GetAllModule(request.PaginationRequest, cancellationToken);
                var allModuleDisplay = _mapper.Map<PagedResult<GetAllModulesResponse>>(allModule.Data);

                return Result<PagedResult<GetAllModulesResponse>>.Success(allModuleDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all company", ex);
            }
        }
    }
}
