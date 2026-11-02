using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.UpdateReceipt
{
    public record UpdateTransactionItemRequest
    (

            decimal amount,
            string remarks,
            string ledgerId

    );
}
