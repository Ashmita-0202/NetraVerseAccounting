using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddJournalEntryDetails
{
    public class AddJournalEntryDetailsCommandHandler : IRequestHandler<AddJournalEntryDetailsCommand, Result<AddJournalEntryDetailsResponse>>
    {
        private readonly IValidator<AddJournalEntryDetailsCommand> _validator;
        private readonly IJournalServices _journalServices;
        private readonly IMapper _mapper;

        public AddJournalEntryDetailsCommandHandler(IMapper mapper,IValidator<AddJournalEntryDetailsCommand> validator, IJournalServices journalServices)
        {
            _journalServices = journalServices;
            _mapper = mapper;
            _validator = validator;
            
        }
        public async Task<Result<AddJournalEntryDetailsResponse>> Handle(AddJournalEntryDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddJournalEntryDetailsResponse>.Failure(errors);
                }

                var addJournalDetails = await _journalServices.AddJournalDetails(request);

                if (addJournalDetails.Errors.Any())
                {
                    var errors = string.Join(", ", addJournalDetails.Errors);
                    return Result<AddJournalEntryDetailsResponse>.Failure(errors);
                }

                if (addJournalDetails is null || !addJournalDetails.IsSuccess)
                {
                    return Result<AddJournalEntryDetailsResponse>.Failure(" ");
                }

                var journalDisplays = _mapper.Map<AddJournalEntryDetailsResponse>(request);
                return Result<AddJournalEntryDetailsResponse>.Success(journalDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding journal entry details", ex);


            }
        }
    }
}
