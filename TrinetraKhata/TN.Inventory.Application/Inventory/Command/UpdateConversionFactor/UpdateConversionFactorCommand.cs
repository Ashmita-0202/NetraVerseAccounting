using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.UpdateConversionFactor
{
    public record UpdateConversionFactorCommand
    (
            string id,
            string fromUnit,
            string toUnit,
            decimal conversionFactor,
            DateTime createdAt
    ):IRequest<Result<UpdateConversionFactorResponse>>;
}
