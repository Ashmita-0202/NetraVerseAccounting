using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.UpdateItem
{
  public record UpdateItemCommand
  (
      string id,
           string name,
            decimal? price,
            string itemGroupId,
            string unitId,
            string? sellingPrice,
            string? costPrice,
            string? barCodeField,
            string? expiredDate,
            decimal? openingStockQuantity,
            string? hasCode,
            decimal? minimumLevel,
            bool? hsSerial,
            string? conversionFactorId,
            bool? isItems,
            bool? isVatEnables,
            bool? isConversionFactor,
            string? stockCenterId,
            List<string>? serialNumbers

  ) :IRequest<Result<UpdateItemResponse>>;
}
