using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;

namespace TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt
{
    public  class ReceiptExcelCommandHandler:IRequestHandler<ReceiptExceCommand,Result<ReceiptExcelResponse>>
    {
        private readonly IReceiptServices _receiptServices;
        private readonly IMapper _mapper;
        private readonly IValidator<ReceiptExceCommand> _validator;

        public ReceiptExcelCommandHandler(IReceiptServices receiptServices,IMapper mapper,IValidator<ReceiptExceCommand> validator)
        {
            _receiptServices = receiptServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<ReceiptExcelResponse>> Handle(ReceiptExceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<ReceiptExcelResponse>.Failure(errors);
                }

                var receiptExcel = await _receiptServices.AddReceiptExcel(request.formFile);

                if (receiptExcel.Errors.Any())
                {
                    var errors = string.Join(", ", receiptExcel.Errors);
                    return Result<ReceiptExcelResponse>.Failure(errors);
                }

                if (receiptExcel is null || !receiptExcel.IsSuccess)
                {
                    return Result<ReceiptExcelResponse>.Failure(" ");
                }

                return Result<ReceiptExcelResponse>.Success(receiptExcel.Message);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Receipt", ex);


            }
        }
    }
}
