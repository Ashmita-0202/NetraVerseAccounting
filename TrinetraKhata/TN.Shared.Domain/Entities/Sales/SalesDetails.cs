using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.StockCenterEntities;
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
            string? date,
            string? billNumber,
            string ledgerId,
            string amountInWords,
            decimal? discountPercent,
            decimal? discountAmount,
            decimal? vatPercent,
            decimal? vatAmount,
            string createdBy,
            string companyId,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            decimal grandTotalAmount,
            SalesStatus status,
            string? journalEntriesId,
            string? paymentId,
            bool isSales,
            string stockCenterId,
            bool isActive,
              decimal? subTotalAmount,
            decimal? taxableAmount,
            decimal? amountAfterVat,
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
            Status = status;
            JournalEntriesId = journalEntriesId;
            PaymentId = paymentId;
            IsSales = isSales;
            StockCenterId = stockCenterId;
            IsActive = isActive;
            SubTotalAmount = subTotalAmount;
            TaxableAmount = taxableAmount;
            AmountAfterVat = amountAfterVat;
            SalesReturnDetails = new List<SalesReturnDetails>();
            //Payments = new List<Payment>();
        }

        public string? Date { get; set; }
        public string? BillNumber { get; set; }
        public string LedgerId { get; set; }

        public string AmountInWords { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? VatPercent { get; set; }
        public decimal? VatAmount { get; set; }

        public string CreatedBy { get; set; }
       
        public string CompanyId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public decimal GrandTotalAmount { get; set; }

        public SalesStatus Status { get; set; } = SalesStatus.Settled;

        public string? JournalEntriesId { get; set; }

        public string? PaymentId { get; set; }
        public bool? IsActive { get; set; }

        public string? StockCenterId { get; set; }
        public virtual StockCenter StockCenter { get; set; }

        public bool IsSales { get; set; }
        public JournalEntry JournalEntry { get; set; }

        public decimal? SubTotalAmount { get; set; }
        public decimal? TaxableAmount { get; set; }
        public decimal? AmountAfterVat { get; set; }

        public enum SalesStatus
        {
            Settled = 1,
            PartiallyReturned = 2,
            Returned = 3
        }

        public ICollection<SalesItems> SalesItems { get; set; }

        public ICollection<SalesReturnDetails> SalesReturnDetails { get; set;}
        //public ICollection<Payment> Payments { get; set; }
    }
}
