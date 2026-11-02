using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Purchase
{
    public class PurchaseQuotationItems : Entity
    {
        public PurchaseQuotationItems() : base(null)
        {

        }

        public PurchaseQuotationItems(
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
            bool isDeleted,
            string purchaseQuotationDetailsId

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
            PurchaseQuotationDetailsId = purchaseQuotationDetailsId;
            IsDeleted = isDeleted;
        }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string UnitId { get; set; }
        //public virtual Units Unit { get; set; }

        public string ItemId { get; set; }
        public virtual Items Item { get; set; }

        public string PurchaseQuotationDetailsId { get; set; }
        public virtual PurchaseQuotationDetails PurchaseQuotationDetails { get; set; }
        public ICollection<ItemInstance> ItemInstances { get; set; }


    }
}
