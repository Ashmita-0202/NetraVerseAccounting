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

namespace TN.Inventory.Application.Inventory.Command.AddConversionFactor
{
    public class AddConversionFactorCommandHandler:IRequestHandler<AddConversionFactorCommand, Result<AddConversionFactorResponse>>
    {
        private readonly IConversionFactorServices _conversionFactorServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddConversionFactorCommand> _validator;

        public AddConversionFactorCommandHandler(IConversionFactorServices conversionFactorServices,IMapper mapper,IValidator<AddConversionFactorCommand> validator)
        { 
             _conversionFactorServices=conversionFactorServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<AddConversionFactorResponse>> Handle(AddConversionFactorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddConversionFactorResponse>.Failure(errors);
                }

                var addConversionFactor = await _conversionFactorServices.AddConversionFactor(request);

                if (addConversionFactor.Errors.Any())
                {
                    var errors = string.Join(", ", addConversionFactor.Errors);
                    return Result<AddConversionFactorResponse>.Failure(errors);
                }

                if (addConversionFactor is null || !addConversionFactor.IsSuccess)
                {
                    return Result<AddConversionFactorResponse>.Failure(" ");
                }

                var conversionFactorDisplays = _mapper.Map<AddConversionFactorResponse>(request);
                return Result<AddConversionFactorResponse>.Success(conversionFactorDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding conversion factor", ex);


            }
        }
    }
}
