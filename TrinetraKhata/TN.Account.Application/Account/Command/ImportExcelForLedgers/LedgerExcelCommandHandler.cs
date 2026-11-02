using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.ImportExcelForLedgers
{
    public  class LedgerExcelCommandHandler: IRequestHandler<LedgerExcelCommand, Result<LedgerExcelResponse>>
    {
        private readonly ILedgerService _ledgerService;
        private readonly IMapper _mapper;
        private readonly IValidator<LedgerExcelCommand> _validator;

        public LedgerExcelCommandHandler(ILedgerService ledgerService,IMapper mapper,IValidator<LedgerExcelCommand> validator)
        {
            _ledgerService = ledgerService;
            _mapper = mapper;
            _validator = validator;


        }

        public async Task<Result<LedgerExcelResponse>> Handle(LedgerExcelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<LedgerExcelResponse>.Failure(errors);
                }

                var ledgerExcel = await _ledgerService.AddLedgerExcel(request.formFile);

                if (ledgerExcel.Errors.Any())
                {
                    var errors = string.Join(", ", ledgerExcel.Errors);
                    return Result<LedgerExcelResponse>.Failure(errors);
                }

                if (ledgerExcel is null || !ledgerExcel.IsSuccess)
                {
                    return Result<LedgerExcelResponse>.Failure(" ");
                }

                return Result<LedgerExcelResponse>.Success(ledgerExcel.Message);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Ledger", ex);


            }
        }
    }
}
