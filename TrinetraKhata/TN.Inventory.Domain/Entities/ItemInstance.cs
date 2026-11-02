using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Primitive;

namespace TN.Inventory.Domain.Entities
{
    public class ItemInstance : Entity
    {
        public ItemInstance(): base(null)
        {
            
        }

        public ItemInstance(
            string id,
            string itemsId,
            string? serialNumber,
            string? purchaseItemsId,
            string? salesItemsId,
            DateTime date
            
            ): base(id)
        {
            ItemsId = itemsId;
            SerialNumber = serialNumber;
            PurchaseItemsId = purchaseItemsId;
            SalesItemsId = salesItemsId;
            Date = date;


        }

        public string ItemsId { get; set; }

        public virtual Items Items { get; set; }
        public string? SerialNumber { get; set; }
        public string? PurchaseItemsId { get; set;}

        public string? SalesItemsId { get; set; }

        public DateTime Date { get; set; }
  
    }
}
