using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;
using static TN.Inventory.Domain.Entities.Inventories;

namespace TN.Inventory.Domain.Entities
{
    public class InventoriesLogs : Entity
    {
        public InventoriesLogs(): base(null)
        {
            
        }

        public InventoriesLogs(
            string id,
            string itemId,
            decimal quantityIn,
            decimal amountIn,
            DateTime entryDate,
            string ledgerId,
            string companyId,
            InventoriesType type,
            DateTime logDate,
            string userId,
            string purchaseItemsId
            ) : base(id)
        {
            ItemId = itemId;
            QuantityIn = quantityIn;
            AmountIn = amountIn;
            EntryDate = entryDate;
            LedgerId = ledgerId;
            CompanyId = companyId;
            Type = type;
            LogDate = logDate;
            UserId = userId;
            PurchaseItemsId = purchaseItemsId;
            
        }

        public string ItemId { get; set; }
        public decimal QuantityIn { get; set; }
        public decimal AmountIn { get; set; }
        public DateTime EntryDate { get; set; }
        public string LedgerId { get; set; }
        public string CompanyId { get; set;}
        public InventoriesType Type { get; set; }   
        public DateTime LogDate { get; set; }
        public string UserId { get; set; }
        public string PurchaseItemsId { get; set; }


    }
}
