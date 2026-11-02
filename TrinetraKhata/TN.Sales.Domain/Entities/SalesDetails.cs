using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Sales.Domain.Entities
{
    public class SalesDetails: Entity
    {
        public SalesDetails(): base(null)
        {
            
        }

        public SalesDetails(
             string id,
            string date,
            string billNumber,
            string ledgerId,
            string amountInWords,
            string discountPercent,
            string discountAmount,
            string vatPercent,
            string vatAmount,
            string createdBy,
            string companyId,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string grandTotalAmount,

            List<SalesItems> salesItems
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
            SalesItems = salesItems;
            CreatedBy = createdBy;
            CompanyId = companyId;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            GrandTotalAmount = grandTotalAmount;
            SalesReturnDetails = new List<SalesReturnDetails>();


        }

        public string Date { get; set; }
        public string BillNumber { get; set; }
        public string LedgerId { get; set; }

        public string AmountInWords { get; set; }
        public string DiscountPercent { get; set; }
        public string DiscountAmount { get; set; }
        public string VatPercent { get; set; }
        public string VatAmount { get; set; }

        public string CreatedBy { get; set; }
       
        public string CompanyId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string GrandTotalAmount { get; set; }

        //public virtual Company Company{get;set;}

        public ICollection<SalesItems> SalesItems { get; set; }

        public ICollection<SalesReturnDetails> SalesReturnDetails { get; set;}
    }
}
