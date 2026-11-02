using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.Primitive;

namespace TN.Purchase.Domain.Entities
{
    public class PurchaseReturnDetails : Entity
    {
        public PurchaseReturnDetails(
            ) : base(null)
        {
            
        }

        public PurchaseReturnDetails(
            string id,
            string purchaseDetailsId,
            string? returnDate,
            decimal totalReturnAmount,
            decimal? taxAdjustment,
            decimal? discount,
            decimal netReturnAmount,
            string companyId,
            string createdBy,
            string reason,
            string ledgerId,
            string? stockCenterId,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
             string? journalEntriesId,
             string? purchaseReturnNumber,
            List<PurchaseReturnItems> purchaseReturnItems

            ) : base(id)
        {
            PurchaseDetailsId = purchaseDetailsId;
            ReturnDate = returnDate;
            TotalReturnAmount = totalReturnAmount;
            TaxAdjustment = taxAdjustment;
            Discount = discount;
            NetReturnAmount = netReturnAmount;
            CompanyId = companyId;
            CreatedBy = createdBy;
            Reason = reason;
            LedgerId = ledgerId;
            StockCenterId = stockCenterId;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            PurchaseReturnNumber = purchaseReturnNumber;
            JournalEntriesId = journalEntriesId;
            PurchaseReturnItems = purchaseReturnItems;
        }

        public string PurchaseDetailsId { get; set; }

        public string? ReturnDate { get; set; }

        public decimal TotalReturnAmount {  get; set; }
        public decimal? TaxAdjustment { get; set; }

        public decimal? Discount { get; set; }

        public decimal NetReturnAmount { get;set; }

        public string CompanyId { get; set; }

        public string Reason { get; set; }
        public string LedgerId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public string? StockCenterId { get; set; }
        public virtual StockCenter StockCenter { get; set; }

        public string? JournalEntriesId { get; set; }

        public string? PurchaseReturnNumber { get; set; }
        public virtual PurchaseDetails PurchaseDetails { get; set; }

        public ICollection<PurchaseReturnItems> PurchaseReturnItems { get; set; }
    }
}
