using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.DeletePermission
{
    public class DeletePermissionCommandHandler:IRequestHandler<DeletePermissionCommand, Result<bool>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public DeletePermissionCommandHandler(IUserServices userServices, IMapper mapper) 
        {
            _userServices = userServices;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deletePermission = await _userServices.DeletePermission(request.id, cancellationToken);
                if (deletePermission is null)
                {
                    return Result<bool>.Failure("NotFound", "Permission not Found");
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
