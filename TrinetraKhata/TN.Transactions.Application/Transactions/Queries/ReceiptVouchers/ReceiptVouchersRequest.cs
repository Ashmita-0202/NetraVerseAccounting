using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Queries.ReceiptVouchers
{
    public record ReceiptVouchersRequest
   (
             decimal? amount,
            string remarks,
            string ledgerId
        );
}
