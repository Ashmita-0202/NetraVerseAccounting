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

namespace TN.Inventory.Application.Inventory.Command.UpdateUnits
{
   public class UpdateUnitsCommandHandler:IRequestHandler<UpdateUnitsCommand, Result<UpdateUnitsResponse>>
    {
        private readonly IUnitsServices _unitsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUnitsCommand> _validator;

        public UpdateUnitsCommandHandler(IUnitsServices unitsServices, IMapper mapper,IValidator<UpdateUnitsCommand> validator) 
        {
            _unitsServices=unitsServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateUnitsResponse>> Handle(UpdateUnitsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateUnitsResponse>.Failure(errors);

                }

                var updateUnits = await _unitsServices.UpdateUnits(request.id, request);

                if (updateUnits.Errors.Any())
                {
                    var errors = string.Join(", ", updateUnits.Errors);
                    return Result<UpdateUnitsResponse>.Failure(errors);
                }

                if (updateUnits is null || !updateUnits.IsSuccess)
                {
                    return Result<UpdateUnitsResponse>.Failure("Updates units failed");
                }

                var unitsDisplay = _mapper.Map<UpdateUnitsResponse>(updateUnits.Data);
                return Result<UpdateUnitsResponse>.Success(unitsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating units by {request.id}", ex);
            }
        }
    }
}
