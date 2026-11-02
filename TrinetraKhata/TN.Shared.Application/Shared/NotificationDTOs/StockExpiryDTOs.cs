using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.NotificationDTOs
{
    public record StockExpiryDTOs
    (
        DateTime? expiredDate,
        string? itemName,
        string? companyId,
        string? stockCenterId
    //int? daysToExpire,
    //string? description,
    //string? stockCenterId
    );
}
