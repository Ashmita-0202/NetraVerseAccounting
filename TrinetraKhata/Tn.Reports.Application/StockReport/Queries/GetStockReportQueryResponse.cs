using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Inventory;

namespace TN.Reports.Application.StockReport.Queries
{
    public record GetStockReportQueryResponse
    (
              string ledgerId,
              string? stockCenterId,
              string? itemId,
              string? itemGroupId,
              string unitId,
              decimal quantity,
              decimal price,
               decimal totalValue,
               string? referenceNumber,
              List<BatchNumber>? batchNumbers,
              List<string>? SerialNumbers,
                List<ManufactureAndExpiry>? manufactureAndExpiries

    );
}
