using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using NV.Payment.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace NV.Payment.Application.Payment.Command.UpdatePayment
{
    public class UpdatePaymentMethodCommandHandler:IRequestHandler<UpdatePaymentMethodCommand,Result<UpdatePaymentMethodResponse>>
    {
        private readonly IPaymentMethodService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePaymentMethodCommand> _validator;

        public UpdatePaymentMethodCommandHandler(IPaymentMethodService service,IMapper mapper, IValidator<UpdatePaymentMethodCommand> validator)
        {
            _service=service;
            _mapper=mapper;
            _validator=validator;


        }

        public async Task<Result<UpdatePaymentMethodResponse>> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePaymentMethodResponse>.Failure(errors);

                }

                var updatePaymentMethod = await _service.Update(request.id, request);

                if (updatePaymentMethod.Errors.Any())
                {
                    var errors = string.Join(", ", updatePaymentMethod.Errors);
                    return Result<UpdatePaymentMethodResponse>.Failure(errors);
                }

                if (updatePaymentMethod is null || !updatePaymentMethod.IsSuccess)
                {
                    return Result<UpdatePaymentMethodResponse>.Failure("Updates paymentMethod failed");
                }

                var paymentMethodDisplay = _mapper.Map<UpdatePaymentMethodResponse>(updatePaymentMethod.Data);
                return Result<UpdatePaymentMethodResponse>.Success(paymentMethodDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating Payment method by {request.id}", ex);
            }
        }
    }
}
