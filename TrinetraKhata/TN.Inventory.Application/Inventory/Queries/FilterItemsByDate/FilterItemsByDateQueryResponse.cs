using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;

namespace TN.Inventory.Application.Inventory.Queries.FilterItemsByDate
{
    public record  FilterItemsByDateQueryResponse
    (
         string id,
        string name,
        decimal? price,
        string itemGroupId,
        string unitId,
        string sellingPrice,
        string costPrice,
        string barCodeField,
        List<DateTime> expiredDate,
        decimal? openingStockQuantity,
        string hsCode,
        bool? hasSerial,
        bool? isItems,
        bool? isVatEnables,
        bool? isConversionFactor,
        string? stockCenterId

    );
}
