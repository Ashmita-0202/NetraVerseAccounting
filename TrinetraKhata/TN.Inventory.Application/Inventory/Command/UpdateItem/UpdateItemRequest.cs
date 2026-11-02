

namespace TN.Inventory.Application.Inventory.Command.UpdateItem
{
   public record UpdateItemRequest
    (
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

   );
}
