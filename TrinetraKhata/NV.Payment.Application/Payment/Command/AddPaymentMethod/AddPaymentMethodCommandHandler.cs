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

namespace NV.Payment.Application.Payment.Command.AddPayment
{
    public  class AddPaymentMethodCommandHandler:IRequestHandler<AddPaymentMethodCommand, Result<AddPaymentMethodResponse>>
    {
        private readonly IPaymentMethodService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<AddPaymentMethodCommand> _validator;

        public AddPaymentMethodCommandHandler(IPaymentMethodService service, IMapper mapper,IValidator<AddPaymentMethodCommand> validator) 
        {
            _service=service;
            _mapper=mapper;
            _validator=validator;

        }

        public async Task<Result<AddPaymentMethodResponse>> Handle(AddPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddPaymentMethodResponse>.Failure(errors);
                }

                var addPaymentMethod = await _service.Add(request);

                if (addPaymentMethod.Errors.Any())
                {
                    var errors = string.Join(", ", addPaymentMethod.Errors);
                    return Result<AddPaymentMethodResponse>.Failure(errors);
                }

                if (addPaymentMethod is null || !addPaymentMethod.IsSuccess)
                {
                    return Result<AddPaymentMethodResponse>.Failure(" ");
                }

                var paymentMethodDisplay = _mapper.Map<AddPaymentMethodResponse>(addPaymentMethod.Data);
                return Result<AddPaymentMethodResponse>.Success(paymentMethodDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding PaymentMethod", ex);

            }
        }
    }
}
