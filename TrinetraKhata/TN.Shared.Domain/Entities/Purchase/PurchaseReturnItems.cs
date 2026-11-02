using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Purchase.Domain.Entities
{
    public class PurchaseReturnItems: Entity
    {
        public PurchaseReturnItems(
            ) : base(null)
        {
            
        }

        public PurchaseReturnItems(
            string id,
            string purchaseReturnDetailsId,
            string purchaseItemsId,
            decimal returnQuantity,
            decimal returnUnitPrice,
            decimal returnTotalAmount,
            string createdBy,
            string createdAt,
            string updatedBy,
            string updatedAt


            ) : base(id)
        {
            PurchaseReturnDetailsId = purchaseReturnDetailsId;
            PurchaseItemsId = purchaseItemsId;
            ReturnQuantity = returnQuantity;
            ReturnUnitPrice = returnUnitPrice;
            ReturnTotalAmount = returnTotalAmount;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
        }

        public string PurchaseReturnDetailsId { get; set; }

        public string PurchaseItemsId { get; set; }

        public decimal ReturnQuantity { get; set; }

        public decimal ReturnUnitPrice { get; set; }

        public decimal ReturnTotalAmount { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

        public virtual PurchaseItems PurchaseItems { get; set; }

        public virtual PurchaseReturnDetails PurchaseReturnDetails { get; set; }
    }
}
