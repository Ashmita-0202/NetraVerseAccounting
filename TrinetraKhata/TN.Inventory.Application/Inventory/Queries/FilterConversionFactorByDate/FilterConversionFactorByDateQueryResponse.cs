using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate
{
    public record FilterConversionFactorByDateQueryResponse
    (
             string id,
             string name,
             string fromUnit,
             string toUnit,
             decimal conversionFactor,
            DateTime createdAt,
            string userId,
            string updateBy,
            DateTime updatedAt
    );
}
