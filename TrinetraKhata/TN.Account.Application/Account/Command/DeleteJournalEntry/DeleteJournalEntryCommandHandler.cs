using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.DeleteJournalEntry
{
   public class DeleteJournalEntryCommandHandler:IRequestHandler<DeleteJournalEntryCommand,Result<bool>>
    {
        private readonly IJournalServices _journalServices;
        private readonly IMapper _mapper;

        public DeleteJournalEntryCommandHandler(IJournalServices journalServices, IMapper mapper) 
        {
            _journalServices=journalServices;
            _mapper = mapper;
        }

        public async Task<Result<bool>> Handle(DeleteJournalEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteJournal = await _journalServices.Delete(request.id, cancellationToken);
                if (deleteJournal is null)
                {
                    return Result<bool>.Failure("NotFound", "Journal not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
