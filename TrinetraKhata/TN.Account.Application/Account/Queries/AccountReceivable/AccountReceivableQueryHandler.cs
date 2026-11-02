using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.AccountPayable;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.AccountReceivable
{
    public sealed class AccountReceivableQueryHandler : IRequestHandler<AccountReceivableQuery, Result<PagedResult<AccountReceivableQueryResponse>>>
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public AccountReceivableQueryHandler(ILedgerService ledgerService, IMapper mapper)
        {
            _ledgerService = ledgerService;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<AccountReceivableQueryResponse>>> Handle(AccountReceivableQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var accountChart = await _ledgerService.GetAccountReceivable(request.PaginationRequest,request.ledgerId);
                var accountReceivable = _mapper.Map<PagedResult<AccountReceivableQueryResponse>>(accountChart.Data);
                return Result<PagedResult<AccountReceivableQueryResponse>>.Success(accountReceivable);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all customer", ex);
            }
        }
    }
}
