
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.StockCenterEntities;
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
            string? date,
            string? billNumber,
            string ledgerId,
            string amountInWords,
            decimal? discountPercent,
            decimal? discountAmount,
            decimal? vatPercent,
            decimal? vatAmount,
            string companyId,
            decimal grandTotalAmount,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            PurchaseStatus status,
            string? referenceNumber,
            string? journalEntriesId,
            bool isDeleted,
            string? paymentId,
            string? stockCenterId,
            bool isPurchase,
            decimal? subTotalAmount,
            decimal? taxableAmount,
            decimal? amountAfterVat,
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
            Status = status;
            ReferenceNumber = referenceNumber;
            JournalEntriesId = journalEntriesId;
            IsDeleted = isDeleted;
            PaymentId = paymentId;
            StockCenterId = stockCenterId;
            IsPurchase = isPurchase;
            SubTotalAmount = subTotalAmount;
            TaxableAmount = taxableAmount;
            AmountAfterVat = amountAfterVat;
            PurchaseReturnDetails = new List<PurchaseReturnDetails>();
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
        public string CompanyId { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy {  get; set; }
        public DateTime ModifiedAt { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? JournalEntriesId { get; set; }
        public bool IsDeleted { get; set; }
        public JournalEntry JournalEntry { get; set; }
        public PurchaseStatus Status { get; set; } = PurchaseStatus.Settled;

        //public virtual Company Company{get;set;}
        public enum PurchaseStatus
        {
            Settled=1,
            PartiallyReturned=2,
            Returned=3
        }

        public bool IsPurchase { get; set; }
        public string? PaymentId { get; set; }
        public string? StockCenterId { get; set; }
        public decimal? SubTotalAmount { get; set; }
        public decimal? TaxableAmount { get; set; }
        public decimal? AmountAfterVat { get; set; }
        public virtual StockCenter StockCenter { get; set; }
        public ICollection<PurchaseItems> PurchaseItems { get; set; }

        public ICollection<PurchaseReturnDetails> PurchaseReturnDetails { get; set; }
        public ICollection<Payments> Payments { get; set; }



    }
}
