using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.SharedEntities.CommonEntities
{
    public class Items : Entity
    {
        public Items(
            ) : base(null)
        {

        }

        public Items(
            string id,
            string name,
            decimal price,
            string itemsGroupId,
            string unitId,
            double openingStockQuantity
            ) : base(id)
        {
            Name = name;
            Price = price;
            ItemGroupId = itemsGroupId;
            UnitId = unitId;
            OpeningStockQuantity = openingStockQuantity;

        }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public string ItemGroupId { get; set; }
        public ItemGroup ItemGroup { get; set; }
        public string UnitId { get; set; }
        public Units Unit { get; set; }
        public double OpeningStockQuantity { get; set; }
        public virtual ICollection<PurchaseItems> PurchaseDetails { get; set; }
    }
}
