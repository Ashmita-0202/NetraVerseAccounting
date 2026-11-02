using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.JournalEntryById
{
    public class GetJournalEntryByIdQueryHandler:IRequestHandler<GetJournalEntryByIdQuery, Result<GetJournalEntryByIdResponse>>
    {
        private readonly IJournalServices _journalServices;
        private readonly IMapper _mapper;

        public GetJournalEntryByIdQueryHandler(IJournalServices journalServices, IMapper mapper)
        {
            _journalServices=journalServices;
            _mapper=mapper;
        }

        public async Task<Result<GetJournalEntryByIdResponse>> Handle(GetJournalEntryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var journalById = await _journalServices.GetJournalById(request.id);
                return Result<GetJournalEntryByIdResponse>.Success(journalById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching journal by using id", ex);
            }
        }
    }
}
