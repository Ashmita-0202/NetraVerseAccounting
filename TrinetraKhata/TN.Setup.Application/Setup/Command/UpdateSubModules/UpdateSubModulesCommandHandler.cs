using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateSubModules
{
   public  class UpdateSubModulesCommandHandler : IRequestHandler<UpdateSubModulesCommand,Result<UpdateSubModulesResponse>>
    {
        private readonly IValidator<UpdateSubModulesCommand> _validator;
        private readonly ISubModules _subModules;
        private readonly IMapper _mapper;

        public UpdateSubModulesCommandHandler(IValidator<UpdateSubModulesCommand> validator,ISubModules subModules,IMapper mapper) 
        {
            _validator=validator;
            _subModules=subModules;
            _mapper=mapper;
        
        }

        public async Task<Result<UpdateSubModulesResponse>> Handle(UpdateSubModulesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSubModulesResponse>.Failure(errors);

                }

                var updateSubModules = await _subModules.Update(request.id, request);

                if (updateSubModules.Errors.Any())
                {
                    var errors = string.Join(", ", updateSubModules.Errors);
                    return Result<UpdateSubModulesResponse>.Failure(errors);
                }

                if (updateSubModules is null || !updateSubModules.IsSuccess)
                {
                    return Result<UpdateSubModulesResponse>.Failure("Updates modules failed");
                }

                var subModulesDisplay = _mapper.Map<UpdateSubModulesResponse>(updateSubModules.Data);
                return Result<UpdateSubModulesResponse>.Success(subModulesDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating SubModules", ex);

            }
        }
    }
}
