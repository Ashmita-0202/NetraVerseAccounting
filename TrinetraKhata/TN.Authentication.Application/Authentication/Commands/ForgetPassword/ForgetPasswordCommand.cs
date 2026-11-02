using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.ForgetPassword
{
    public record ForgetPasswordCommand
    (
        string email
        ): IRequest<Result<ForgetPasswordResponse>>;
}
