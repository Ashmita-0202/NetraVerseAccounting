using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Notification
{
    public class StockExpiryNotification : Entity
    {
        public StockExpiryNotification(): base(null)
        {
            
        }

        public StockExpiryNotification(
            string id,
            DateTime? expiredDate,
        string? itemName,
        string? companyId,
        int? daysToExpire,
        string? description,
        string? stockCenterId
            ) : base(id)
        {
            ExpiredDate = expiredDate;
            ItemName = itemName;
            CompanyId = companyId;
            DaysToExpire = daysToExpire;
            Description = description;
            StockCenterId = stockCenterId;

        }
        public  DateTime? ExpiredDate { get; set; }
        public string? ItemName { get; set; }
            public string? CompanyId { get; set; }
        public int? DaysToExpire { get; set; }
        public string? Description { get; set; }

        public string? StockCenterId { get; set; }

    }
}
