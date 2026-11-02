using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.DeleteUser
{
public  class DeleteUserCommandHandler :IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IUserServices userServices,IMapper mapper)
        {
            _userServices=userServices;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteUser = await _userServices.Delete(request.userId, cancellationToken);
                if (deleteUser is null)
                {
                    return Result<bool>.Failure("NotFound", "User not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.userId}", ex);
            }
        }
    }
}
