using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.UpdateDate
{
    public record  UpdateDateCommand
    (

        string userId,
        DateTime Date
    ):IRequest<Result<UpdateDateResponse>>;
}
