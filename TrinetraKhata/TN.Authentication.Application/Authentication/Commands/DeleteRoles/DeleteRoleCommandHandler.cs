using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.DeleteRoles
{
 public  class DeleteRoleCommandHandler:IRequestHandler<DeleteRoleCommand,Result<bool>>
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IMapper _mapper;

        public DeleteRoleCommandHandler(IAuthenticationServices authenticationServices,IMapper mapper) 
        {
           _authenticationServices=authenticationServices;
            _mapper=mapper;
        
        }

        public async Task<Result<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteRole = await _authenticationServices.Delete(request.id, cancellationToken);
                if (deleteRole is null)
                {
                    return Result<bool>.Failure("NotFound", "User not Found");
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
