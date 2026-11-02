using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        
        string Username,
        string Email,
        string Password,
        string CompanyName,
        string Address,
        string CompanyShortName,
        string ContactNumber,
        string ContactPerson,
        string PAN




        ) : IRequest<Result<RegisterResponse>>;
    
}
