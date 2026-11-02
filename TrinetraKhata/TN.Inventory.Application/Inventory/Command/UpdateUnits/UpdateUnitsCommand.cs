using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.UpdateUnits
{
 public record UpdateUnitsCommand
    (
             string id,
            string name,
            bool isEnabled
     ) :IRequest<Result<UpdateUnitsResponse>>;
}
