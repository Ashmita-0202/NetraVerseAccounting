using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Sales
{
    public class SalesQuotationDetails : Entity
    {
        public SalesQuotationDetails() : base(null)
        {

        }
        public SalesQuotationDetails(
            string id,
            string? date,
            string? quotationNumber,
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
            bool isActive,
            string stockCenterId,
            string? salesQuotationNumber,
            decimal? subTotalAmount,
            decimal? taxableAmount,
            decimal? amountAfterVat,
            QuotationStatus quotationStatus,
            List<SalesQuotationItems> salesQuotationItems
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
          
            CreatedBy = createdBy;
            CompanyId = companyId;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            GrandTotalAmount = grandTotalAmount;
            IsActive = isActive;
            StockCenterId = stockCenterId;
            SalesQuotationNumber = salesQuotationNumber;
            QuotationStatuss = quotationStatus;
            SubTotalAmount = subTotalAmount;
            TaxableAmount = taxableAmount;
            AmountAfterVat = amountAfterVat;

            SalesQuotationItems = salesQuotationItems ?? new List<SalesQuotationItems>(); ;
        }

        public string? Date { get; set; }
        public string? QuotationNumber { get; set; }
        public string LedgerId { get; set; }

        public string AmountInWords { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? VatPercent { get; set; }
        public decimal? VatAmount { get; set; }

        public string? SalesQuotationNumber { get; set; }
        public string CreatedBy { get; set; }

        public string CompanyId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public decimal GrandTotalAmount { get; set; }

        public decimal? SubTotalAmount { get; set; }
        public decimal? TaxableAmount { get; set; }
        public decimal? AmountAfterVat { get; set; }

        public bool? IsActive { get; set; }
        public QuotationStatus QuotationStatuss { get; set; } = QuotationStatus.Pending;

        public string? StockCenterId { get; set; }
        public virtual StockCenter StockCenter { get; set; }
        public ICollection<SalesQuotationItems> SalesQuotationItems { get; set; } = new List<SalesQuotationItems>();

        public enum QuotationStatus
        {
            Pending,
            Converted
        }

    }
}
