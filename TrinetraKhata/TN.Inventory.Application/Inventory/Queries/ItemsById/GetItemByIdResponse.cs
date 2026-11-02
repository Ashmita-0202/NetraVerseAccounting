using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.ItemsById
{
    public record GetItemByIdResponse
        (
            string id = "",
        string name = "",
        decimal? price = null,
        string itemGroupId = "",
        string unitId = "",
        string sellingPrice = "",
        string costPrice = "",
        string barCodeField = "",
        string expiredDate = "",
        double? openingStockQuantity = null,
        string serialNumber = "",
        string hsCode="",
        bool? hasSerial=true,
        bool? isItems = true,
        bool? isVatEnables = true,
        string conversionFactorId = "",
        bool? isConversionFactor = true,
        string? stockCenterId = "",
        bool? hasExpiryAndManufacture = false,
        bool? hasBatchNumber = false,

         List<string>? serialNumbers = null


        );
   
}
