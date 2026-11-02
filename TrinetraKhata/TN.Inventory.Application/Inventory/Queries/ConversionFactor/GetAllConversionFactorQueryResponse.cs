using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.ConversionFactor
{
    public record GetAllConversionFactorQueryResponse
        (
         string id="",
         string name="",
         string fromUnit = "",
         string toUnit = "",
         decimal conversionFactor= 0,
            DateTime createdAt= default,
            string userId = "",
            string updateBy="", 
            DateTime updatedAt = default
        );

}
