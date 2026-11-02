using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Purchase.Domain.Entities
{
    public class PurchaseItems : Entity
    {
        public PurchaseItems(): base(null)
        {
            
        }

        public PurchaseItems(
            string id,
            decimal quantity,
            string unitId,
            string itemId,
            decimal price,
            decimal amount,
            string createdBy,
            string createdAt,
            string updatedBy,
            string updatedAt,
            string purchaseDetailsId
          
            ) : base(id)
        {
            Id = id;
            Quantity = quantity;
            UnitId = unitId;
            ItemId = itemId;
            Price = price;
            Amount = amount;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            PurchaseDetailsId = purchaseDetailsId;
            PurchaseReturnItems = new List<PurchaseReturnItems>();
            ItemInstances = new List<ItemInstance>();
        }

        public decimal Quantity { get; set; }
       
        public decimal Price { get; set; }
        public decimal Amount { get; set; } 
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set;}
        public string UpdatedAt { get; set;}

        public string UnitId { get; set; }
        //public virtual Units Unit { get; set; }

        public string ItemId { get; set; }
        //public virtual Items Item { get; set; }

        public string PurchaseDetailsId { get; set; }
        public virtual PurchaseDetails PurchaseDetails { get; set; }

        public ICollection<PurchaseReturnItems> PurchaseReturnItems { get; set; }
        public ICollection<ItemInstance> ItemInstances { get; set; }

    }
}
