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



namespace TN.Account.Application.Account.Command.AddLedgerGroup
{
    public class AddLedgerGroupCommandHandler : IRequestHandler<AddLedgerGroupCommand, Result<AddLedgerGroupResponse>>
    {
        private readonly ILedgerGroupService _ledgerGroupService;
        private readonly IValidator<AddLedgerGroupCommand> _validator;
        private readonly IMapper _mapper;

        public AddLedgerGroupCommandHandler(IValidator<AddLedgerGroupCommand> validator, IMapper mapper, ILedgerGroupService ledgerGroupService)
        {
            _ledgerGroupService = ledgerGroupService;
            _validator = validator;
            _mapper = mapper;

        }

        public async Task<Result<AddLedgerGroupResponse>> Handle(AddLedgerGroupCommand request, CancellationToken cancellationToken)
        {
            try {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddLedgerGroupResponse>.Failure(errors);
                }

                var addLedgerGroup = await _ledgerGroupService.Add(request);

                if (addLedgerGroup.Errors.Any())
                {
                    var errors = string.Join(", ", addLedgerGroup.Errors);
                    return Result<AddLedgerGroupResponse>.Failure(errors);

                }

                if (addLedgerGroup is null || !addLedgerGroup.IsSuccess)
                {
                    return Result<AddLedgerGroupResponse>.Failure(" ");
                }

                var ledgerGroupDisplays = _mapper.Map<AddLedgerGroupResponse>(request);
                return Result<AddLedgerGroupResponse>.Success(ledgerGroupDisplays);

            } catch (Exception ex) 
            {
                throw new Exception("An error occurred while adding Ledger Group", ex);

            };


        }
    }
}
