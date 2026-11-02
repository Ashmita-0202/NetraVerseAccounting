using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.Entities.Purchase;
using TN.Shared.Domain.Entities.Sales;
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
            string? salesQuotationItemsId,
            string? purchaseQuotationItemsId,
            decimal? rate,
            DateTime date
            
            ): base(id)
        {
            ItemsId = itemsId;
            SerialNumber = serialNumber;
            PurchaseItemsId = purchaseItemsId;
            SalesItemsId = salesItemsId;
            SalesQuotationItemsId = salesQuotationItemsId;
            PurchaseQuotationItemsId = purchaseQuotationItemsId;
            Rate = rate;
            Date = date;


        }

        public string ItemsId { get; set; }

        public virtual Items Items { get; set; }
        public string? SerialNumber { get; set; }
        public string? PurchaseItemsId { get; set;}

        public decimal? Rate { get; set; }
        public virtual PurchaseItems PurchaseItems { get; set; }

        public string? SalesQuotationItemsId { get; set; }

        public virtual SalesQuotationItems SalesQuotationItems { get; set; }


        public string? PurchaseQuotationItemsId { get; set; }

        public virtual PurchaseQuotationItems PurchaseQuotationItems { get; set; }

        public SalesItems SalesItems { get; set; }
        public string? SalesItemsId { get; set; }

        public DateTime Date { get; set; }
  
    }
}
