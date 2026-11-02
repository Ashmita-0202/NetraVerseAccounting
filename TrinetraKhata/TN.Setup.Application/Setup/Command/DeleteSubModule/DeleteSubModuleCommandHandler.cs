using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.DeleteSubModule
{
    public class DeleteSubModuleCommandHandler:IRequestHandler<DeleteSubModuleCommand, Result<bool>>
    {
        private readonly ISubModules _subModules;

        public DeleteSubModuleCommandHandler(ISubModules subModules)
        {
            _subModules=subModules;
            
        }

        public async Task<Result<bool>> Handle(DeleteSubModuleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSubModule = await _subModules.Delete(request.id, cancellationToken);
                if (deleteSubModule is null)
                {
                    return Result<bool>.Failure("NotFound", "SubModules not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
