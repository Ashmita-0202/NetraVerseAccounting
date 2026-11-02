using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Application.Transactions.Command.AddTransactions;

namespace TN.Transactions.Application.Transactions.Command.AddReceipt
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Result<AddReceiptResponse>>
    {
        private readonly IReceiptServices _receiptServices;
        private readonly IValidator<AddReceiptCommand> _validator;
        private readonly IMapper _mapper;

        public AddReceiptCommandHandler(IReceiptServices receiptServices,
            IValidator<AddReceiptCommand> validator,
            IMapper mapper)
        {
            _receiptServices = receiptServices;
            _validator = validator;
            _mapper = mapper;
            
        }
        public async Task<Result<AddReceiptResponse>> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddReceiptResponse>.Failure(errors);
                }

                var addTransactions = await _receiptServices.Add(request);

                if (addTransactions.Errors.Any())
                {
                    var errors = string.Join(", ", addTransactions.Errors);
                    return Result<AddReceiptResponse>.Failure(errors);
                }

                if (addTransactions is null || !addTransactions.IsSuccess)
                {
                    return Result<AddReceiptResponse>.Failure(" ");
                }

                var addTransactionsResponse = _mapper.Map<AddReceiptResponse>(addTransactions.Data);
                return Result<AddReceiptResponse>.Success(addTransactionsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Transactions", ex);


            }
        }
    }
}
