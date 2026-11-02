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
            decimal returnTotalPrice

            ): base(id)
        {
            SalesReturnDetailsId = salesReturnDetailsId;
            SalesItemsId = salesItemsId;
            ReturnQuantity = returnQuantity;
            ReturnUnitPrice = returnUnitPrice;
            ReturnTotalPrice = returnTotalPrice;
            
        }

        public string SalesReturnDetailsId { get; set; }

        public string SalesItemsId { get; set; }

        public decimal ReturnQuantity { get; set; }

        public decimal ReturnUnitPrice { get; set; }

        public decimal ReturnTotalPrice { get; set; }

        public virtual SalesItems SalesItems { get; set; }
        public virtual SalesReturnDetails SalesReturnDetails { get; set; }
    }
}
