using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.Customer;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.GetBalance
{
    public sealed class GetBalanceByQueryHandler : IRequestHandler<GetBalanceByQuery, Result<GetBalanceByQueryResponse>>
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public GetBalanceByQueryHandler(ILedgerService ledgerService, IMapper mapper)
        {
            _ledgerService = ledgerService;
            _mapper = mapper;
            
        }
        public async Task<Result<GetBalanceByQueryResponse>> Handle(GetBalanceByQuery request, CancellationToken cancellationToken=default)
        {
            try
            {
                var balance = await _ledgerService.GetBalance(request.ledgerId);
                var balanceDisplay = _mapper.Map<GetBalanceByQueryResponse>(balance.Data);
                return Result<GetBalanceByQueryResponse>.Success(balanceDisplay);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all customer", ex);
            }
        }
    }
}
