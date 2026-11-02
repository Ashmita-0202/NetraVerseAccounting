using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.UpdateUser
{
    public record UpdateUserCommand
        (
            string Id,
            string FirstName,
            string LastName,
            string UserName,
            string Address,
            string Email

        ) :IRequest<Result<UpdateUserResponse>>;
    
}
