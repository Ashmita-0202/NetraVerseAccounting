using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Sales.Domain.Entities
{
    public class SalesReturnDetails: Entity
    {
        public SalesReturnDetails(): base(null)
        {
            
        }

        public SalesReturnDetails(
             string id,
            string salesDetailsId,
            DateTime returnDate,
            decimal totalReturnAmount,
            decimal taxAdjustment,
            decimal netReturnAmount,
            string reason,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            SalesDetailsId = salesDetailsId;
            ReturnDate = returnDate;
            TotalReturnAmount = totalReturnAmount;
            TaxAdjustment = taxAdjustment;
            NetReturnAmount = netReturnAmount;
            Reason = reason;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            SalesReturnItems = new List<SalesReturnItems>();

        }


        public string SalesDetailsId { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal TotalReturnAmount { get; set; }

        public decimal TaxAdjustment { get; set; }

        public decimal NetReturnAmount { get;set; }

        public string Reason { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public virtual SalesDetails SalesDetails{get;set;}

        public ICollection<SalesReturnItems> SalesReturnItems { get; set; }
    }
}
