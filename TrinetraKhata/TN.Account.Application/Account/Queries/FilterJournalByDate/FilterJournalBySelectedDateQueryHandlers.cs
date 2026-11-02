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

namespace TN.Account.Application.Account.Queries.FilterJournalByDate
{
    public class FilterJournalBySelectedDateQueryHandler : IRequestHandler<FilterJournalBySelectedDateQuery, Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>>
    {
        private readonly IJournalServices _journalServices;
        private readonly IMapper _mapper;

        public FilterJournalBySelectedDateQueryHandler(IJournalServices journalServices, IMapper mapper)
        {
            _journalServices = journalServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>> Handle(FilterJournalBySelectedDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
            
                var filterJournal = await _journalServices.GetJournalFilter(request.PaginationRequest,request.FilterJournalDTOs);
                if (!filterJournal.IsSuccess || filterJournal.Data == null)
                {
                    return Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>.Failure(filterJournal.Message);
                }
                var filterJournalResult = _mapper.Map<PagedResult<FilterJournalBySelectedDateQueryResponse>>(filterJournal.Data);

                return Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>.Success(filterJournalResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>.Failure(
                    $"An error occurred while fetching journal entries by date: {ex.Message}");
            }
        }
    }
}
