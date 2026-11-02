using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.FilterItemsByDate
{
    public record  FilterItemsDTOs
    (
        string? name,
        string? startDate,
        string? endDate
    );
}
