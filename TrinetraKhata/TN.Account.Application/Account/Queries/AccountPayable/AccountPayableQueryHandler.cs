using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.ChartOfAccounts;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.AccountPayable
{
    public sealed class AccountPayableQueryHandler : IRequestHandler<AccountPayableQuery, Result<ARAPWithTotals>>
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public AccountPayableQueryHandler(ILedgerService ledgerService, IMapper mapper)
        {
            _ledgerService = ledgerService;
            _mapper = mapper;
            
        }

        public async Task<Result<ARAPWithTotals>> Handle(AccountPayableQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var arapDetails = await _ledgerService.GetAccountPayable(request.PaginationRequest, request.ledgerId);
                var accountPayable = _mapper.Map<ARAPWithTotals>(arapDetails.Data);
                return Result<ARAPWithTotals>.Success(accountPayable);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all customer", ex);
            }
        }
    }
}
