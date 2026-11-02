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

namespace TN.Setup.Application.Setup.Command.AddModule
{
    public class AddModuleCommandHandler : IRequestHandler<AddModuleCommand, Result<AddModuleResponse>>
    {
        private readonly IValidator<AddModuleCommand> _validator;
        private readonly IModule _module;
        private readonly IMapper _mapper;

        public AddModuleCommandHandler(IValidator<AddModuleCommand> validator, IModule module, IMapper mapper)
        {
            _validator = validator;
            _module = module;
            _mapper = mapper;
            
        }
        public async Task<Result<AddModuleResponse>> Handle(AddModuleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if(!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddModuleResponse>.Failure(errors);
                }

                var addModules = await _module.Add(request);
                if(addModules.Errors.Any())
                {
                    var errors = string.Join(", ", addModules.Errors);
                    return Result<AddModuleResponse>.Failure(errors);

                }

                if (addModules is null || !addModules.IsSuccess)
                {
                    return Result<AddModuleResponse>.Failure("Add modules Failed");
                }

                var moduleDisplays = _mapper.Map<AddModuleResponse>(request);
                return Result<AddModuleResponse>.Success(moduleDisplays);

            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while Adding Modules");
            }
        }
    }
}
