using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.Account.Command.UpdateLedgerGroup;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateLedger
{
    public  class UpdateLedgerCommandHandler: IRequestHandler<UpdateLedgerCommand,Result<UpdateLedgerResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ILedgerService _ledgerService;
        private readonly IValidator<UpdateLedgerCommand> _validator;

        public UpdateLedgerCommandHandler(IValidator<UpdateLedgerCommand> validator, ILedgerService ledgerService, IMapper mapper) 
        {
            _mapper = mapper;
            _ledgerService = ledgerService;
            _validator = validator;

        }

        public async Task<Result<UpdateLedgerResponse>> Handle(UpdateLedgerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateLedgerResponse>.Failure(errors);

                }

                var updateLedger = await _ledgerService.Update(request.id, request);

                if (updateLedger.Errors.Any())
                {
                    var errors = string.Join(", ", updateLedger.Errors);
                    return Result<UpdateLedgerResponse>.Failure(errors);
                }

                if (updateLedger is null || !updateLedger.IsSuccess)
                {
                    return Result<UpdateLedgerResponse>.Failure("Updates Ledger Group failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateLedgerResponse>(updateLedger.Data);
                return Result<UpdateLedgerResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating Ledger by {request.id}", ex);
            }
        }
    }
}
