using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;


//using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.master
{
    public sealed class GetAllMasterByQueryHandler : IRequestHandler<GetAllMasterByQuery, Result<PagedResult<GetAllMasterByQueryResponse>>>
    {
        private readonly IMasterService _Services;
        private readonly IMapper _mapper;

        public GetAllMasterByQueryHandler(IMasterService masterServices, IMapper mapper)
        {
            _Services = masterServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllMasterByQueryResponse>>> Handle(GetAllMasterByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allMaster = await _Services.GetAllMaster(request.PaginationRequest, cancellationToken);
                var allMasterDisplay = _mapper.Map<PagedResult<GetAllMasterByQueryResponse>>(allMaster.Data);
                return Result<PagedResult<GetAllMasterByQueryResponse>>.Success(allMasterDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching allMaster", ex);
            }
        }
    }

}
