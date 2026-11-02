using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.AddItemGroup
{
    public record AddItemGroupCommand
    (
            string name,
            string? description,
            bool isPrimary,
            string? itemsGroupId
    ) :IRequest<Result<AddItemGroupResponse>>;
}
