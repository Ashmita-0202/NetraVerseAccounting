using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.Parties
{
    public sealed class GetAllPartiesByQueryHandler : IRequestHandler<GetAllPartiesByQuery, Result<PagedResult<GetAllPartiesByQueriesResponse>>>
    {

        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public GetAllPartiesByQueryHandler(ILedgerService ledgerService, IMapper mapper)
        {
            _mapper = mapper;
            _ledgerService = ledgerService;
            
        }
        public async Task<Result<PagedResult<GetAllPartiesByQueriesResponse>>> Handle(GetAllPartiesByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allParties = await _ledgerService.GetAllParties(request.PaginationRequest, cancellationToken);
                var allPartiesDisplay = _mapper.Map<PagedResult<GetAllPartiesByQueriesResponse>>(allParties.Data);
                return Result<PagedResult<GetAllPartiesByQueriesResponse>>.Success(allPartiesDisplay);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all ledger", ex);
            }
        }
    }
}
