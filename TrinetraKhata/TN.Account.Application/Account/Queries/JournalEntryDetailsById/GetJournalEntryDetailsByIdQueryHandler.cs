using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.JournalEntryDetailsById
{
  public sealed class GetJournalEntryDetailsByIdQueryHandler : IRequestHandler<GetJournalEntryDetailsByIdQuery,Result<GetJournalEntryDetailsByIdResponse>>
    {
        private readonly IJournalDetailsServices _journalDetailsServices;
        private readonly IMapper _mapper;

        public GetJournalEntryDetailsByIdQueryHandler(IJournalDetailsServices journalDetailsServices, IMapper mapper)
        {
            _journalDetailsServices=journalDetailsServices;
            _mapper=mapper;
        }

        public async Task<Result<GetJournalEntryDetailsByIdResponse>> Handle(GetJournalEntryDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var journalDetailsById = await _journalDetailsServices.GetJournalDetailsById(request.id);
                return Result<GetJournalEntryDetailsByIdResponse>.Success(journalDetailsById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Journal by using id", ex);
            }
        }
    }
}
