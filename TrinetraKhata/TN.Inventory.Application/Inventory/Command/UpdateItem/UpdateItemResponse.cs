
namespace TN.Inventory.Application.Inventory.Command.UpdateItem
{
    public record UpdateItemResponse
    (
        string Id="",
            string Name = "",
            decimal? Price = 0,
            string ItemGroupId = "",
            string UnitId = "",
            string SellingPrice = "",
            string CostPrice = "",
            string BarCodeField = "",
            string ExpiredDate = "",
            decimal? OpeningStockQuantity = 0,
            string hsCode = "",
            string? conversionFactorId = "",
            decimal? minimumLevel=0,
            bool? isItems = false,
            bool? isVatEnables = false,
            bool? isConversionFactor = true,
            string? stockCenterId = "",
             List<string>? serialNumbers = null

    );
}
