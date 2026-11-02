using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Inventory
{
    public class BatchNumber : Entity
    {
        public BatchNumber(): base(null)
        {

        }

        public BatchNumber(
            string id,
            string? batchNumber,
            decimal? totalQuantity,
            string? itemId
            ) :base(id)
        {
            BatchNumbers = batchNumber;
            TotalQuantity = totalQuantity;
            ItemId = itemId;

        }
        public string? BatchNumbers { get; set; }
        public decimal? TotalQuantity { get; set; }
        public string? ItemId { get; set; }
        public Items? Item { get; set; }
    }
}
