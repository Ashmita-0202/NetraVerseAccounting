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
using TN.Transactions.Application.Transactions.Command.AddReceipt;

namespace TN.Transactions.Application.Transactions.Command.AddPayments
{
    public class AddPaymentsCommandHandler : IRequestHandler<AddPaymentsCommand, Result<AddPaymentsResponse>>
    {

        private readonly IPaymentsServices _paymentsServices;
        private readonly IValidator<AddPaymentsCommand> _validator;
        private readonly IMapper _mapper;

        public AddPaymentsCommandHandler(IPaymentsServices paymentsServices, IValidator<AddPaymentsCommand> validator, IMapper mapper)
        {
            _paymentsServices = paymentsServices;
            _validator = validator;
            
        }



        public async Task<Result<AddPaymentsResponse>> Handle(AddPaymentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddPaymentsResponse>.Failure(errors);
                }

                var addPayments = await _paymentsServices.Add(request);

                if (addPayments.Errors.Any())
                {
                    var errors = string.Join(", ", addPayments.Errors);
                    return Result<AddPaymentsResponse>.Failure(errors);
                }

                if (addPayments is null || !addPayments.IsSuccess)
                {
                    return Result<AddPaymentsResponse>.Failure(" ");
                }
                if (addPayments.Data == null)
                {
                    return Result<AddPaymentsResponse>.Failure("No payment data returned.");
                }

                //var addPaymentsResponse = _mapper.Map<AddPaymentsResponse>(addPayments.Data);

                return Result<AddPaymentsResponse>.Success(addPayments.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Transactions", ex);
            }
        }
    }
}
