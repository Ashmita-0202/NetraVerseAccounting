using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Sales.Application.Sales.Queries.CurrentSalesBillNumber;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.CurrentJournalReferenceNumber
{
    public  class CurrentJournalRefNoQueryHandler : IRequestHandler<CurrentJournalRefNoQuery, Result<CurrentJournalRefNoQueryResponse>>
    {
        private readonly IJournalDetailsServices _journalDetailsServices;

        public CurrentJournalRefNoQueryHandler(IJournalDetailsServices journalDetailsServices,IMapper mapper)
        {
            _journalDetailsServices = journalDetailsServices;
        }

        public async Task<Result<CurrentJournalRefNoQueryResponse>> Handle(CurrentJournalRefNoQuery request, CancellationToken cancellationToken)
        {
            var result = await _journalDetailsServices.GetCurrentJournalRefNo();
            if (result.IsSuccess)
            {
                return Result<CurrentJournalRefNoQueryResponse>.Success(new CurrentJournalRefNoQueryResponse(result.Message));
            }
            else
            {
                return Result<CurrentJournalRefNoQueryResponse>.Failure("Reference number is not generated");
            }
        }
    }
}
