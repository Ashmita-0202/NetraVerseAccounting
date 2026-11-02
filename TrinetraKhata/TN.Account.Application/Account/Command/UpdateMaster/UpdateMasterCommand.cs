using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateMaster
{
    public record  UpdateMasterCommand
        (
            string id,
            string name
       
        ):IRequest<Result<UpdateMasterResponse>>;
    
}
