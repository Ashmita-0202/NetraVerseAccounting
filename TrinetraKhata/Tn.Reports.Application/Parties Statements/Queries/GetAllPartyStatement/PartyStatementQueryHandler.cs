using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.Parties_Statements.Queries
{
    public class PartyStatementQueryHandler : IRequestHandler<PartyStatementQuery, Result<List<PartyStatementQueryResponse>>>
    {
        private readonly IPartyStatementServices _partyStatementServices;
        private readonly IMapper _mapper;

        public PartyStatementQueryHandler(IPartyStatementServices partyStatementServices, IMapper mapper)
        {
            _partyStatementServices = partyStatementServices;
            _mapper = mapper;
        }
        public async Task<Result<List<PartyStatementQueryResponse>>> Handle(PartyStatementQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _partyStatementServices.GetPartyStatement(request.partyId);

                var partyStatementResult = _mapper.Map<List<PartyStatementQueryResponse>>(result.Data);

                return Result<List<PartyStatementQueryResponse>>.Success(partyStatementResult);
            }
            catch (Exception ex)
            {
                return Result<List<PartyStatementQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
