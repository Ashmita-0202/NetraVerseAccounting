using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddItems
{
    public record AddItemRequest
    (
            string name,
            decimal? price,
            string itemGroupId,
            string unitId,
            string? sellingPrice,
            string? costPrice,
            string? barCodeField,
            decimal? openingStockQuantity,
            string? hsCode,
            decimal? minimumLevel,
            bool? hasSerial,
            string? conversionFactorId,
            bool? isItems,
            bool? isVatEnables,
            bool? isConversionFactor,
       
            string? stockCenterId,
             bool? hasExpiryAndManufacture,
                bool? hasBatchNumber,
              List<ExpiryAndManufactureDTOs>? manufactureAndExpiries,
                List<BatchNumberDTOs>? batchNumbers,

             List<string>? serialNumbers
    );
}
