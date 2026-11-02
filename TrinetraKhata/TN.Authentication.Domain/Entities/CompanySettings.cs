using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class CompanySettings: Entity
    {
        public CompanySettings(): base(null)
        {
            
        }

        public CompanySettings(
            string id,
            bool showExpiredDateInItem,
            bool showSerialNumberInItem,
            bool showSerialNumberForPurchase,
            bool showSerialNumberForSales,
            JournalReferencesType journalReferences,
            InventoryMethodType inventoryMethod,
            string companyId
            ): base(id)
        {
            ShowExpiredDateInItem = showExpiredDateInItem;
            ShowSerialNumberInItem = showSerialNumberInItem;
            ShowSerialNumberForPurchase = showSerialNumberForPurchase;
            ShowSerialNumberForSales = showSerialNumberForSales;
            JournalReference = journalReferences;
            InventoryMethod = inventoryMethod;
            CompanyId = companyId;
                  
        }

        public bool ShowExpiredDateInItem { get; set; }
        public bool ShowSerialNumberInItem { get;set; }

        public bool ShowSerialNumberForPurchase { get; set; }
        public bool ShowSerialNumberForSales { get; set; }

        public JournalReferencesType JournalReference { get; set; } = JournalReferencesType.Automatic;
        public InventoryMethodType InventoryMethod { get; set; } = InventoryMethodType.FIFO;
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public enum JournalReferencesType
        {
            Manual,
            Automatic
        }

        public enum InventoryMethodType
        {
            FIFO=1,
            LIFO = 2,
            AverageWeighted=3
        }
    }
}
