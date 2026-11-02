using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddItemGroup
{
    public record AddItemGroupResponse
    (
            string name,
            string? description,
            bool isPrimary,
            string? itemsGroupId
    );
}
