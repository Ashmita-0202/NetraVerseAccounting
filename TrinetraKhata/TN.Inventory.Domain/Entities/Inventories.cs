using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Inventory.Domain.Entities
{
    public class Inventories : Entity
    {
        public Inventories(): base(null)
        {
            
        }

        public Inventories(
            string id,
            string itemId,
            decimal quantityIn,
            decimal amountIn,
            DateTime entryDate,
            decimal quantityOut,
            decimal amountOut,
            string ledgerId,
            bool? isOpeningStock,
            InventoriesType type,
            string CompanyId,
            string unitId,
            string purchaseItemsId,
             string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {

            ItemId = itemId;
            QuantityIn = quantityIn;
            AmountIn = amountIn;
            EntryDate = entryDate;
            QuantityOut = quantityOut;
            AmountOut = amountOut;
            LedgerId = ledgerId;
            IsOpeningStuck = isOpeningStock;
            Type = type;
            companyId = CompanyId;
            UnitId = unitId;
            PurchaseItemsId = purchaseItemsId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
        }
        public string ItemId { get; set; }
        public decimal QuantityIn { get; set; }
        public decimal AmountIn { get; set; }

        public Items Items { get; set; } = null!;

        public DateTime EntryDate { get; set; }

        public string companyId { get; set; }
        public string UnitId { get; set; }
        public bool? IsOpeningStuck { get; set; }

        public InventoriesType Type { get; set; }
        public string LedgerId { get; set; }

        public decimal QuantityOut { get; set; }
        public decimal AmountOut { get; set; }
        public string PurchaseItemsId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public enum InventoriesType
        {
            None = 0,
            Purchase = 1,
            Sales = 2
        }
    }
}
