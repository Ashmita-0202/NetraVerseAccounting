using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.UpdateConversionFactor
{
    public class UpdateConversionFactorCommandHandler : IRequestHandler<UpdateConversionFactorCommand, Result<UpdateConversionFactorResponse>>
    {

        private readonly IConversionFactorServices _conversionFactorServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateConversionFactorCommand> _validator;

        public UpdateConversionFactorCommandHandler(IConversionFactorServices conversionFactorServices,IMapper mapper,IValidator<UpdateConversionFactorCommand> validator)
        {
            _conversionFactorServices=conversionFactorServices;
            _mapper = mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateConversionFactorResponse>> Handle(UpdateConversionFactorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateConversionFactorResponse>.Failure(errors);

                }

                var updateConversionFactor = await _conversionFactorServices.UpdateConversionFactor(request.id, request);

                if (updateConversionFactor.Errors.Any())
                {
                    var errors = string.Join(", ", updateConversionFactor.Errors);
                    return Result<UpdateConversionFactorResponse>.Failure(errors);
                }

                if (updateConversionFactor is null || !updateConversionFactor.IsSuccess)
                {
                    return Result<UpdateConversionFactorResponse>.Failure("Updates units failed");
                }

                var Display = _mapper.Map<UpdateConversionFactorResponse>(updateConversionFactor.Data);
                return Result<UpdateConversionFactorResponse>.Success(Display);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating units by {request.id}", ex);
            }
        }

    }

}
