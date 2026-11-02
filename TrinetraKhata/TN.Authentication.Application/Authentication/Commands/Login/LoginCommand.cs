using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.Login
{
    public record LoginCommand(string username, string password) : IRequest<Result<LoginResponse>>;
   
}
