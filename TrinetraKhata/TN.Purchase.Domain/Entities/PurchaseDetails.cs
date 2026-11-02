using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Purchase.Domain.Entities
{
    public class PurchaseDetails : Entity
    {
        public PurchaseDetails() : base(null)
        {

        }

        public PurchaseDetails(
            string id,
            string date,
            string billNumber,
            string ledgerId,
            string amountInWords,
            string discountPercent,
            string discountAmount,
            string vatPercent,
            string vatAmount,
            string companyId,
            decimal grandTotalAmount,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            List<PurchaseItems> purchaseItems
            ) : base(id)
        {
            Date = date;
            BillNumber = billNumber;
            LedgerId = ledgerId;
            AmountInWords = amountInWords;
            DiscountPercent = discountPercent;
            DiscountAmount = discountAmount;
            VatPercent = vatPercent;
            VatAmount = vatAmount;
            CompanyId = companyId;
            GrandTotalAmount = grandTotalAmount;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            PurchaseItems = purchaseItems;
            PurchaseReturnDetails = new List<PurchaseReturnDetails>();
           


        }

        public string Date { get; set; }
        public string BillNumber { get; set; }
        public string LedgerId { get; set; }

        public string AmountInWords { get; set; }
        public string DiscountPercent { get; set; }
        public string DiscountAmount { get; set; }
        public string VatPercent { get; set; }
        public string VatAmount { get; set; }
        public string CompanyId { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy {  get; set; }
        public DateTime ModifiedAt { get; set; }

        //public virtual Company Company{get;set;}

        public ICollection<PurchaseItems> PurchaseItems { get; set; }

        public ICollection<PurchaseReturnDetails> PurchaseReturnDetails { get; set; }

        



    }
}
