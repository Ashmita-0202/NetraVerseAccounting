using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Inventory
{
    public class ManufactureAndExpiry : Entity
    {
        public ManufactureAndExpiry(): base(null)
        {
            
        }

        public ManufactureAndExpiry(
            string id,
            DateTime? expiredDate,
            DateTime? manufacturingDate,
            decimal? totalQuantity,
            string itemId
            ) : base(id)
        {
            ExpiredDate = expiredDate;
            ManufacturingDate = manufacturingDate;
            TotalQuantity = totalQuantity;
            ItemId = itemId;

        }
        public  DateTime? ExpiredDate { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public decimal? TotalQuantity { get; set; }
        public string ItemId { get; set; }
        public Items Item { get; set; }

    }
}
