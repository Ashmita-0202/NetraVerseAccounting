using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.ConversionFactorById
{
    public record GetConversionFactorByIdResponse
    (
            string id="",
            string name="",
            string fromUnit = "",
            string toUnit = "",
            decimal conversionFactors = 0,
            DateTime createdAt= default,
            string userId = "",
            string updateBy="",
            DateTime updatedAt= default
    );
}
