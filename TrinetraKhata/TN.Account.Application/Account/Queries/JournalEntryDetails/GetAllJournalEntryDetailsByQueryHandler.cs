using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.JournalEntryDetails
{
    public sealed class GetAllJournalEntryDetailsByQueryHandler :IRequestHandler<GetAllJournalEntryDetailsByQuery, Result<PagedResult<GetAllJournalEntryDetailsByQueryResponse>>>
        {
        private readonly IJournalDetailsServices _journalDetailsServices;
        private readonly IMapper _mapper;

        public GetAllJournalEntryDetailsByQueryHandler(IJournalDetailsServices journalDetailsServices, IMapper mapper)
        {
            _journalDetailsServices=journalDetailsServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllJournalEntryDetailsByQueryResponse>>> Handle(GetAllJournalEntryDetailsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allJournal = await _journalDetailsServices.GetAllJournalDetails(request.PaginationRequest, cancellationToken);
                var allJournalDisplay = _mapper.Map<PagedResult<GetAllJournalEntryDetailsByQueryResponse>>(allJournal.Data);
                return Result<PagedResult<GetAllJournalEntryDetailsByQueryResponse>>.Success(allJournalDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all journal", ex);
            }
        }
    }
}
