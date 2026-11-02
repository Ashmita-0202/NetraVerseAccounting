using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.master;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.LedgerGroup
{
    public sealed class GetAllLedgerGroupByQueryHandler : IRequestHandler<GetAllLedgerGroupByQuery, Result<PagedResult<GetAllLedgerGroupByQueryResponse>>>
    {
        private readonly ILedgerGroupService _services;
        private readonly IMapper _mapper;

        public GetAllLedgerGroupByQueryHandler(ILedgerGroupService ledgerGroupService,IMapper mapper)
        {
            _services=ledgerGroupService;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllLedgerGroupByQueryResponse>>> Handle(GetAllLedgerGroupByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allLedgerGroup = await _services.GetAllLedgerGroup(request.PaginationRequest, cancellationToken);
                var allLedgerGroupDisplay = _mapper.Map<PagedResult<GetAllLedgerGroupByQueryResponse>>(allLedgerGroup.Data);
                return Result<PagedResult<GetAllLedgerGroupByQueryResponse>>.Success(allLedgerGroupDisplay);


            }
            catch (Exception ex) 
            {

                throw new Exception("An error occured while fetching all ledger group", ex);
            }
        }
    }
}
