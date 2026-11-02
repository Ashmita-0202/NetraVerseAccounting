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

namespace TN.Account.Application.Account.Queries.OpeningClosingBalance
{
    public class OpeningClosingBalanceQueryHandler : IRequestHandler<OpeningClosingBalanceQuery, Result<PagedResult<OpeningClosingBalanceResponse>>>
    {
        private readonly IOpeningClosingBalanceServices _openingClosingBalanceServices;
        private readonly IMapper _mapper;

        public OpeningClosingBalanceQueryHandler(IOpeningClosingBalanceServices openingClosingBalanceServices, IMapper mapper)
        {
            _mapper = mapper;
            _openingClosingBalanceServices = openingClosingBalanceServices;

        }
        public async Task<Result<PagedResult<OpeningClosingBalanceResponse>>> Handle(OpeningClosingBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allOpeningClosingBalance = await _openingClosingBalanceServices.GetOpeningClosingBalance(request.fyId, request.paginationRequest, cancellationToken);
                var allOpeningClosingBalanceDisplays = _mapper.Map<PagedResult<OpeningClosingBalanceResponse>>(allOpeningClosingBalance.Data);
                return Result<PagedResult<OpeningClosingBalanceResponse>>.Success(allOpeningClosingBalanceDisplays);

            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching opening closing balance", ex);
            }
        }
    }
}
