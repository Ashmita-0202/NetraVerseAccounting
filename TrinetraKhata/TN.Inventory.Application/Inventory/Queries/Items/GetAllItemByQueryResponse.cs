using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.Items
{
    public record GetAllItemByQueryResponse
        (
        string id="",
        string name="",
        decimal? price=0,
        string itemGroupId= "",
        string unitId="",
        string sellingPrice="",
        string costPrice = "",
        string barCodeField = "",
        string expiredDate = "",
        decimal? openingStockQuantity=0,
        string hsCode = "",
        bool? hasSerial = true,
        bool? isItems = true,
        bool? isVatEnables = true,
        bool? isConversionFactor = true,
        string? stockCenterId = ""
        );
   
}
