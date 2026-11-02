using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            DateTime returnDate,
            decimal totalReturnAmount,
            decimal taxAdjustment,
            decimal netReturnAmount,
            string companyId,
            string createdBy,
            string reason,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            List<PurchaseReturnItems> purchaseReturnItems

            ) : base(id)
        {
            PurchaseDetailsId = purchaseDetailsId;
            ReturnDate = returnDate;
            TotalReturnAmount = totalReturnAmount;
            TaxAdjustment = taxAdjustment;
            NetReturnAmount = netReturnAmount;
            CompanyId = companyId;
            CreatedBy = createdBy;
            Reason = reason;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            PurchaseReturnItems = purchaseReturnItems;
        }

        public string PurchaseDetailsId { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal TotalReturnAmount {  get; set; }
        public decimal TaxAdjustment { get; set; }

        public decimal NetReturnAmount { get;set; }

        public string CompanyId { get; set; }

        public string Reason { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }


        public virtual PurchaseDetails PurchaseDetails { get; set; }

        public ICollection<PurchaseReturnItems> PurchaseReturnItems { get; set; }
    }
}
