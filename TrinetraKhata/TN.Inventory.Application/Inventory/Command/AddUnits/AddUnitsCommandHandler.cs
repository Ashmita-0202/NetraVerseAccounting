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


namespace TN.Inventory.Application.Inventory.Command.AddUnits
{
    public class AddUnitsCommandHandler: IRequestHandler<AddUnitsCommand, Result<AddUnitsResponse>>
    {
        private readonly IUnitsServices _unitsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddUnitsCommand> _validator;

        public AddUnitsCommandHandler(IUnitsServices unitsServices,IMapper mapper, IValidator<AddUnitsCommand> validator)
        {
            _unitsServices=unitsServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<AddUnitsResponse>> Handle(AddUnitsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddUnitsResponse>.Failure(errors);
                }

                var addUnits = await _unitsServices.AddUnits(request);

                if (addUnits.Errors.Any())
                {
                    var errors = string.Join(", ", addUnits.Errors);
                    return Result<AddUnitsResponse>.Failure(errors);
                }

                if (addUnits is null || !addUnits.IsSuccess)
                {
                    return Result<AddUnitsResponse>.Failure(" ");
                }

                var unitsDisplays = _mapper.Map<AddUnitsResponse>(addUnits.Data);
                return Result<AddUnitsResponse>.Success(unitsDisplays);


            }
            catch (Exception ex)
            {
                throw;


            }
        }
    }
}
