using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.Account.Command.AddLedgerGroup;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddLedger
{
    public class AddLedgerCommandHandler : IRequestHandler<AddLedgerCommand, Result<AddLedgerResponse>>
    {
        private readonly ILedgerService _ledgerService;
        private readonly IValidator<AddLedgerCommand> _validator;
        private readonly IMapper _mapper;

        public AddLedgerCommandHandler(IValidator<AddLedgerCommand> validator, IMapper mapper, ILedgerService ledgerService)
        {

            _ledgerService = ledgerService;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<AddLedgerResponse>> Handle(AddLedgerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddLedgerResponse>.Failure(errors);
                }

                var addLedger = await _ledgerService.Add(request);

                if (addLedger.Errors.Any())
                {
                    var errors = string.Join(", ", addLedger.Errors);
                    return Result<AddLedgerResponse>.Failure(errors);
                }

                if (addLedger is null || !addLedger.IsSuccess)
                {
                    return Result<AddLedgerResponse>.Failure(" ");
                }

                var ledgerDisplays = _mapper.Map<AddLedgerResponse>(addLedger.Data);
                return Result<AddLedgerResponse>.Success(ledgerDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Ledger", ex);


            }
        }
    }
}
