using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.SubledgerGroup
{
    public class  GetAllSubledgerGroupQueryHandler:IRequestHandler<GetAllSubledgerGroupQuery,Result<PagedResult<GetAllSubledgerGroupQueryResposne>>>
    {
        private readonly ISubledgerGroupService _service;
        private readonly IMapper _mapper;

        public GetAllSubledgerGroupQueryHandler(ISubledgerGroupService service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;   

        }

        public async Task<Result<PagedResult<GetAllSubledgerGroupQueryResposne>>> Handle(GetAllSubledgerGroupQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var allSubledgerGroup = await _service.GetAll(request.PaginationRequest, cancellationToken);
                var allSubledgerGroupDisplay = _mapper.Map<PagedResult<GetAllSubledgerGroupQueryResposne>>(allSubledgerGroup.Data);
                return Result<PagedResult<GetAllSubledgerGroupQueryResposne>>.Success(allSubledgerGroupDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all subledgerGroup", ex);
            }

        }
    }
}
