using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.DeleteModule
{
    public class DeleteModuleCommandHandler : IRequestHandler<DeleteModuleCommand, Result<bool>>
    {
        private readonly IModule _module;
        public DeleteModuleCommandHandler(IModule module)
        {
            _module = module;
            
        }

        public async Task<Result<bool>> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteModule = await _module.Delete(request.Id, cancellationToken);
                if (deleteModule is null)
                {
                    return Result<bool>.Failure("NotFound", "Modules not Found");
                }
                return Result<bool>.Success(true);
            

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.Id}", ex);
            }
        }
    }
}
