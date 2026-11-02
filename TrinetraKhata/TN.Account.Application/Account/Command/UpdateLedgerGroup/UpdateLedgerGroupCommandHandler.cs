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

namespace TN.Account.Application.Account.Command.UpdateLedgerGroup
{
    public class UpdateLedgerGroupCommandHandler : IRequestHandler<UpdateLedgerGroupCommand, Result<UpdateLedgerGroupResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ILedgerGroupService _ledgerGroupservices;
        private readonly IValidator<UpdateLedgerGroupCommand> _validator;

        public UpdateLedgerGroupCommandHandler(IValidator<UpdateLedgerGroupCommand> validator, ILedgerGroupService ledgerGroupServices, IMapper mapper)
        {
            _mapper = mapper;
            _ledgerGroupservices = ledgerGroupServices;
            _validator = validator;
        }

        public async Task<Result<UpdateLedgerGroupResponse>> Handle(UpdateLedgerGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateLedgerGroupResponse>.Failure(errors);

                }

                var updateLedgerGroup = await _ledgerGroupservices.Update(request.id, request);

                if (updateLedgerGroup.Errors.Any())
                {
                    var errors = string.Join(", ", updateLedgerGroup.Errors);
                    return Result<UpdateLedgerGroupResponse>.Failure(errors);
                }

                if (updateLedgerGroup is null || !updateLedgerGroup.IsSuccess)
                {
                    return Result<UpdateLedgerGroupResponse>.Failure("Updates Ledger Group failed");
                }

                var ledgerGroupDisplay = _mapper.Map<UpdateLedgerGroupResponse>(updateLedgerGroup.Data);
                return Result<UpdateLedgerGroupResponse>.Success(ledgerGroupDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating LedgerGroup by {request.id}");
            }
        }
    }
}
