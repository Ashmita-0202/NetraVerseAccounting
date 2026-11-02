using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.StockExpiryNotification
{
    public record StockExpiryNotificationResponse
   (
                  DateTime? expiredDate,
        string? itemName,
        string? companyId,
        int? daysToExpire,
        string? description,
        string? stockCenterId

        );
}
