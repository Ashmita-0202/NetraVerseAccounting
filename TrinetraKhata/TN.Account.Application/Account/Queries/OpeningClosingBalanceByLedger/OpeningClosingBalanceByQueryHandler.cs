using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.OpeningStockByCompanyId;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.OpeningClosingBalanceByLedger
{
    public class OpeningClosingBalanceByQueryHandler : IRequestHandler<OpeningClosingBalanceByLedgerQuery, Result<OpeningClosingBalanceByLedgerResponse>>
    {
        private readonly IOpeningClosingBalanceServices _openingClosingBalanceServices;
        private readonly IMapper _mapper;

        public OpeningClosingBalanceByQueryHandler(
            IOpeningClosingBalanceServices openingClosingBalanceServices,
            IMapper mapper
            )
        {
            _openingClosingBalanceServices = openingClosingBalanceServices;
            _mapper = mapper;

        }
   
        public async Task<Result<OpeningClosingBalanceByLedgerResponse>> Handle(OpeningClosingBalanceByLedgerQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var openingClosingBalanceByLedger = await _openingClosingBalanceServices.GetOpeningClosingBalanceByLedger(request.OpeningClosingBalanceDTOs, cancellationToken);

                return Result<OpeningClosingBalanceByLedgerResponse>.Success(openingClosingBalanceByLedger.Data);


            }
            catch (Exception ex)

            {

                throw new Exception("An error occurred while fetching openingstock by companyId", ex);

            }
        }
    }
}
