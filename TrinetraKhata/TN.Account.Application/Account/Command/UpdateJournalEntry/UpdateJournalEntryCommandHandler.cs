using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateJournalEntry
{
   public class UpdateJournalEntryCommandHandler:IRequestHandler<UpdateJournalEntryCommand, Result<UpdateJournalEntryResponse>>
    {
        private readonly IJournalServices _journalServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateJournalEntryCommand> _validator;

        public UpdateJournalEntryCommandHandler(IJournalServices journalServices,IMapper mapper,IValidator<UpdateJournalEntryCommand> validator) 
        {
            _journalServices=journalServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateJournalEntryResponse>> Handle(UpdateJournalEntryCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateJournalEntryResponse>.Failure(errors);

                }

                var updateJournal = await _journalServices.Update(request.journalEntryId, request);

                if (updateJournal.Errors.Any())
                {
                    var errors = string.Join(", ", updateJournal.Errors);
                    return Result<UpdateJournalEntryResponse>.Failure(errors);
                }

                if (updateJournal is null || !updateJournal.IsSuccess)
                {
                    return Result<UpdateJournalEntryResponse>.Failure("Updates Journal Entry failed");
                }

                var journalDisplay = _mapper.Map<UpdateJournalEntryResponse>(updateJournal.Data);
                return Result<UpdateJournalEntryResponse>.Success(journalDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating Journal by {request.id}", ex);
            }
        }
    }
}
