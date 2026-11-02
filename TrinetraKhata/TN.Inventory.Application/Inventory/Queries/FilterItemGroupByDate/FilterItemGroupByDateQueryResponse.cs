using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.FilterItemGroupByDate
{
    public record  FilterItemGroupByDateQueryResponse
   (
             string id,
            string? name,
            string description,
            bool isPrimary,
            string? itemsGroupId
        );
}
