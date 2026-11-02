using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.DeleteMenu
{
    public class DeleteMenuCommandHandler :IRequestHandler<DeleteMenuCommand, Result<bool>>
    {
        private readonly IMenuServices _menuServices;

        public DeleteMenuCommandHandler(IMenuServices menuServices)
        {
            _menuServices=menuServices;
        }

        public async Task<Result<bool>> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteModule = await _menuServices.Delete(request.id, cancellationToken);
                if (deleteModule is null)
                {
                    return Result<bool>.Failure("NotFound", "Menu not Found");
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
