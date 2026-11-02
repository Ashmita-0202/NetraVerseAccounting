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

namespace TN.Account.Application.Account.Command.UpdateJournalEntryDetails
{
 public  class UpdateJournalDetailsCommandHandler: IRequestHandler<UpdateJournalDetailsCommand, Result<UpdateJournalDetailsResponse>>
    {
        private readonly IJournalDetailsServices _journalDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateJournalDetailsCommand> _validator;

        public UpdateJournalDetailsCommandHandler(IJournalDetailsServices journalDetailsServices,IMapper mapper,IValidator<UpdateJournalDetailsCommand> validator)
        {
            _journalDetailsServices=journalDetailsServices;
            _mapper=mapper;
            _validator=validator;

        }

        public async Task<Result<UpdateJournalDetailsResponse>> Handle(UpdateJournalDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateJournalDetailsResponse>.Failure(errors);

                }

                var updateJournalDetails = await _journalDetailsServices.Update(request.id, request);

                if (updateJournalDetails.Errors.Any())
                {
                    var errors = string.Join(", ", updateJournalDetails.Errors);
                    return Result<UpdateJournalDetailsResponse>.Failure(errors);
                }

                if (updateJournalDetails is null || !updateJournalDetails.IsSuccess)
                {
                    return Result<UpdateJournalDetailsResponse>.Failure("Updates journal entry details failed");
                }

                var journalDetailsDisplay = _mapper.Map<UpdateJournalDetailsResponse>(updateJournalDetails.Data);
                return Result<UpdateJournalDetailsResponse>.Success(journalDetailsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating journal entry details by {request.id}", ex);
            }
        }
    }
}
