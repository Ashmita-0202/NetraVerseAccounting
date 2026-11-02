using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.Ledger
{
    public sealed class GetAllLedgerByQueryHandler : IRequestHandler<GetAllLedgerByQuery, Result<PagedResult<GetAllLedgerByQueryResponse>>>
    {
        private readonly ILedgerService _services;
        private readonly IMapper _mapper;

        public GetAllLedgerByQueryHandler(ILedgerService ledgerService,IMapper mapper)
        {
            _services = ledgerService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllLedgerByQueryResponse>>> Handle(GetAllLedgerByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allLedger = await _services.GetAllLedger(request.PaginationRequest, cancellationToken);
                var allLedgerDisplay = _mapper.Map<PagedResult<GetAllLedgerByQueryResponse>>(allLedger.Data);
                return Result<PagedResult<GetAllLedgerByQueryResponse>>.Success(allLedgerDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all ledger", ex);
            }
        }
    }
}
