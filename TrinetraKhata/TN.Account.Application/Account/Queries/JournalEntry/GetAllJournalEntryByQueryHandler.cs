using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.JournalEntry
{
    public sealed class  GetAllJournalEntryByQueryHandler:IRequestHandler<GetallJournalEntryByQuery,Result<PagedResult<GetAllJournalEntryByQueryResponse>>>
    {
        private readonly IJournalServices _journalServices;
        private readonly IMapper _mapper;

        public GetAllJournalEntryByQueryHandler(IJournalServices journalServices, IMapper mapper)
        {
            _journalServices=journalServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllJournalEntryByQueryResponse>>> Handle(GetallJournalEntryByQuery request, CancellationToken cancellationToken)
        {
            try
            {
               
                var result = await _journalServices.GetAllJournalEntriesAsync(request.PaginationRequest);

                var journalEntryResponses = _mapper.Map<PagedResult<GetAllJournalEntryByQueryResponse>>(result.Data);

                return Result<PagedResult<GetAllJournalEntryByQueryResponse>>.Success(journalEntryResponses);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetAllJournalEntryByQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
