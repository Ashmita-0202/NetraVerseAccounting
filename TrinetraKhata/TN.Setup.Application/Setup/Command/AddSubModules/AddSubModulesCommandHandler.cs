using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddSubModules
{
    public class AddSubModulesCommandHandler : IRequestHandler<AddSubModulesCommand, Result<AddSubmodulesResponse>>
    {
        private readonly IValidator<AddSubModulesCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ISubModules _subModules;

        public AddSubModulesCommandHandler(IValidator<AddSubModulesCommand> validator, IMapper mapper, ISubModules subModules)
        {
            _validator = validator;
            _mapper = mapper;
            _subModules = subModules;
            
        }
        public async Task<Result<AddSubmodulesResponse>> Handle(AddSubModulesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if(!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
                    return Result<AddSubmodulesResponse>.Failure(errors);
                }

                var addSubModules = await _subModules.Add(request);

                if(addSubModules.Errors.Any())
                {
                    var errors = string.Join(", ", addSubModules.Errors);
                    return Result<AddSubmodulesResponse>.Failure(errors);
                }

                if(addSubModules is null || !addSubModules.IsSuccess)
                {
                    return Result<AddSubmodulesResponse>.Failure("Add Submodules failed");
                }

                var subModulesDisplay = _mapper.Map<AddSubmodulesResponse>(request);
                return Result<AddSubmodulesResponse>.Success(subModulesDisplay);

            }catch(Exception ex)
            {
                throw new Exception("An error occurred while adding submodules");
            }
        }
    }
}
