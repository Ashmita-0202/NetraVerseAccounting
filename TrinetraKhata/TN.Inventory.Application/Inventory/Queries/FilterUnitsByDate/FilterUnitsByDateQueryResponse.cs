using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.FilterUnitsByDate
{
    public record FilterUnitsByDateQueryResponse
    (
            string id,
            string name,
            DateTime createdAt,
            string userId,
            DateTime updatedAt,
            string updatedBy,
            bool isEnabled

    );
}
