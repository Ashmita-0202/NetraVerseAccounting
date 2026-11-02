using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddReceipt
{
    public record AddTransactionItemsForReceipt
    (
        decimal amount,
            string? remarks,
            string ledgerId,
            decimal? debitAmount,
            decimal? creditAmount
        );
    
}
