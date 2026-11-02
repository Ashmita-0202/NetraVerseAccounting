using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.ARAPByLedgerId
{
    public class ArApByLedgerQueryHandler : IRequestHandler<ArApByLedgerQuery, Result<ArApByLedgerQueryResponse>>
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;

        public ArApByLedgerQueryHandler(ILedgerService ledgerService, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ledgerService = ledgerService ?? throw new ArgumentNullException(nameof(ledgerService));

        }
        public async Task<Result<ArApByLedgerQueryResponse>> Handle(ArApByLedgerQuery request, CancellationToken cancellationToken)
        {
            var arapResponse = await _ledgerService.GetArApByLedger(request.ledgerId, cancellationToken);

            if (arapResponse is not { IsSuccess: true, Data: not null })
                return Result<ArApByLedgerQueryResponse>.Failure(arapResponse?.Message ?? $"Getting Journal Reference Number by ledger {request.ledgerId}");

            return Result<ArApByLedgerQueryResponse>.Success(
                _mapper.Map<ArApByLedgerQueryResponse>(arapResponse.Data));
        }
    }
}
