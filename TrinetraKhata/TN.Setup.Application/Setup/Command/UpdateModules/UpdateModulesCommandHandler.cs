using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateModules
{
    public class UpdateModulesCommandHandler : IRequestHandler<UpdateModulesCommand, Result<UpdateModulesResponse>>
    {
        private readonly IValidator<UpdateModulesCommand> _validator;
        private readonly IModule _module;
        private readonly IMapper _mapper;

        public UpdateModulesCommandHandler(IValidator<UpdateModulesCommand> validator, IModule module, IMapper mapper)
        {
            _mapper = mapper;
            _module = module;
            _validator = validator;
            
        }
        public async Task<Result<UpdateModulesResponse>> Handle(UpdateModulesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x=>x.ErrorMessage));
                    return Result<UpdateModulesResponse>.Failure(errors);

                }

                var updateModules = await _module.Update(request.Id, request);

                if(updateModules.Errors.Any())
                {
                    var errors = string.Join(", ", updateModules.Errors);
                    return Result<UpdateModulesResponse>.Failure(errors);
                }

                if(updateModules is null || !updateModules.IsSuccess)
                {
                    return Result<UpdateModulesResponse>.Failure("Updates modules failed");
                }

                var modulesDisplay = _mapper.Map<UpdateModulesResponse>(updateModules.Data);
                return Result<UpdateModulesResponse>.Success(modulesDisplay);




            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while updating modules");
            }
        }
    }
}
