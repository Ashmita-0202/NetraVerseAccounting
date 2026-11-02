using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddCustomerCategory;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddJournalEntry
{
    public class AddJournalEntryCommandHandler : IRequestHandler<AddJournalEntryCommand, Result<AddJournalEntryResponse>>
    {
        private readonly IJournalServices _journalServices;
        private readonly IValidator<AddJournalEntryCommand> _validator;
        private readonly IMapper _mapper;

        public AddJournalEntryCommandHandler(IMapper mapper,IJournalServices journalServices, IValidator<AddJournalEntryCommand> validator)
        {
            _mapper = mapper;
            _validator = validator;
            _journalServices = journalServices;
            
        }
        public async Task<Result<AddJournalEntryResponse>> Handle(AddJournalEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddJournalEntryResponse>.Failure(errors);
                }

                var addJournal = await _journalServices.Add(request);

                if (addJournal.Errors.Any())
                {
                    var errors = string.Join(", ", addJournal.Errors);
                    return Result<AddJournalEntryResponse>.Failure(errors);
                }

                if (addJournal is null || !addJournal.IsSuccess)
                {
                    return Result<AddJournalEntryResponse>.Failure(" ");
                }

                var addJournalResponse = _mapper.Map<AddJournalEntryResponse>(addJournal.Data);
                return Result<AddJournalEntryResponse>.Success(addJournalResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Journal Entry", ex);


            }
        }
    }
}
