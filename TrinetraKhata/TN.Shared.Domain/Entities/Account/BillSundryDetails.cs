using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Account
{
    public class BillSundryDetails : Entity
    {

        public BillSundryDetails(
            ): base(null)
        {
            
        }

        public BillSundryDetails(
            string id,
            string? billSundryId,
            decimal? rate,
            string? purchaseSalesAndReturnId,
            bool isActive
            ) : base(id)
        {
            BillSundryId = billSundryId;
            Rate = rate;
            PurchaseSalesAndReturnId = purchaseSalesAndReturnId;
            IsActive = isActive;
        }
        public string? BillSundryId { get; set; }

        public decimal? Rate { get; set; }
        public BillSundry? BillSundry { get; set; }
        public string? PurchaseSalesAndReturnId { get; set; }
        public bool IsActive { get; set; }




    }
}
