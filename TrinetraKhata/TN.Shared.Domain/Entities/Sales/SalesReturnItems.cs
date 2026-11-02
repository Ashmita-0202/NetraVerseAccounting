using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Sales.Domain.Entities
{
    public class SalesReturnItems : Entity
    {

        public SalesReturnItems(): base(null)
        {
            
        }

        public SalesReturnItems(
            string id,
            string salesReturnDetailsId,
            string salesItemsId,
            decimal returnQuantity,
            decimal returnUnitPrice,
            decimal returnTotalPrice,
            string createdBy,
            string createdAt,
            string updatedBy,
            string updatedAt

            ) : base(id)
        {
            SalesReturnDetailsId = salesReturnDetailsId;
            SalesItemsId = salesItemsId;
            ReturnQuantity = returnQuantity;
            ReturnUnitPrice = returnUnitPrice;
            ReturnTotalPrice = returnTotalPrice;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            
        }

        public string SalesReturnDetailsId { get; set; }

        public string SalesItemsId { get; set; }

        public decimal ReturnQuantity { get; set; }

        public decimal ReturnUnitPrice { get; set; }

        public decimal ReturnTotalPrice { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

        public virtual SalesItems SalesItems { get; set; }
        public virtual SalesReturnDetails SalesReturnDetails { get; set; }
    }
}
