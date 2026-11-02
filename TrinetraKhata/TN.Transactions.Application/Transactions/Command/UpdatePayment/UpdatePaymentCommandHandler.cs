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
using TN.Transactions.Application.Transactions.Command.UpdateIncome;

namespace TN.Transactions.Application.Transactions.Command.UpdatePayment
{
    public class UpdatePaymentCommandHandler:IRequestHandler<UpdatePaymentCommand, Result<UpdatePaymentResponse>>
    {
        private readonly IPaymentsServices _paymentsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePaymentCommand> _validator;

        public UpdatePaymentCommandHandler(IPaymentsServices paymentsServices,IMapper mapper,IValidator<UpdatePaymentCommand> validator)
        {
            _paymentsServices = paymentsServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdatePaymentResponse>> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePaymentResponse>.Failure(errors);

                }

                var updatePayment = await _paymentsServices.Update(request.id, request);

                if (updatePayment.Errors.Any())
                {
                    var errors = string.Join(", ", updatePayment.Errors);
                    return Result<UpdatePaymentResponse>.Failure(errors);
                }

                if (updatePayment is null || !updatePayment.IsSuccess)
                {
                    return Result<UpdatePaymentResponse>.Failure("Updates payment transaction failed");
                }

                var receiptDisplay = _mapper.Map<UpdatePaymentResponse>(updatePayment.Data);
                return Result<UpdatePaymentResponse>.Success(receiptDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating payment by {request.id}", ex);
            }
        }
    }
}
