using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddItems
{
    public record AddItemResponse
        (
            string Name = "",
            string Price = "",
            string ItemGroupId = "",
            string UnitId = "",
            string SellingPrice="",
            string CostPrice="",
            string BarCodeField="",
            string ExpiredDate="",
            string OpeningStockQuantity = "",
            string hsCode="",
            string? conversionFactorId="",
            bool? isItems = true,
            bool? isVatEnables = true,
            bool? isConversionFactor = true,
            string? stockCenterId="",
            string? manufacturingDate="",
             List<string>? serialNumbers=null
        );
    }
