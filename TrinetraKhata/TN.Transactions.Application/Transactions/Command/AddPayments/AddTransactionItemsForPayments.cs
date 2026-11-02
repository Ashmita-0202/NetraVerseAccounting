using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddPayments
{
    public record AddTransactionItemsForPayments
    (
        decimal amount,
            string remarks,
            string ledgerId
        );
    
}
