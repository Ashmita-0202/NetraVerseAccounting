using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.ItemGroup
{
    public record GetAllItemGroupByQueryResponse
    (        
            string id,
            string name,
            string description,
            bool isPrimary,
            string itemsGroupId
    );
}
