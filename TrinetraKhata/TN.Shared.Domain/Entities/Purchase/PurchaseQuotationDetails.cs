using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Purchase
{
    public class PurchaseQuotationDetails : Entity
    {
        public PurchaseQuotationDetails() : base(null)
        {

        }


        public PurchaseQuotationDetails(
            string id,
            string? date,
            string? quotationNumber,
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
            string? referenceNumber,
            bool isDeleted,
            string? stockCenterId,
            string? purchaseReturnNumber,
                 decimal? subTotalAmount,
     decimal? taxableAmount,
     decimal? amountAfterVat,
            QuotationStatus quotationStatus,
            List<PurchaseQuotationItems> purchaseQuotationItems
            ) : base(id)
        {
            Date = date;
            QuotationNumber = quotationNumber;
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

            ReferenceNumber = referenceNumber;
            IsDeleted = isDeleted;
            StockCenterId = stockCenterId;
            PurchaseQuotationNumber = purchaseReturnNumber;
            QuotationStatuss = quotationStatus;
            SubTotalAmount = subTotalAmount;
            TaxableAmount = taxableAmount;
            AmountAfterVat = amountAfterVat;
            PurchaseQuotationItems = purchaseQuotationItems ?? new List<PurchaseQuotationItems>();




        }

        public string? Date { get; set; }
        public string? QuotationNumber { get; set; }
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
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string? ReferenceNumber { get; set; }

        public bool IsDeleted { get; set; }

        public decimal? SubTotalAmount { get; set; }
        public decimal? TaxableAmount { get; set; }
        public decimal? AmountAfterVat { get; set; }


        public string? PurchaseQuotationNumber { get; set; }
        public QuotationStatus QuotationStatuss { get; set; } = QuotationStatus.Pending;
        public string? StockCenterId { get; set; }
        public virtual StockCenter StockCenter { get; set; }
        public ICollection<PurchaseQuotationItems> PurchaseQuotationItems { get; set; } = new List<PurchaseQuotationItems>();

        public enum QuotationStatus
        {
            Pending,
            Converted
        }





    }
}
