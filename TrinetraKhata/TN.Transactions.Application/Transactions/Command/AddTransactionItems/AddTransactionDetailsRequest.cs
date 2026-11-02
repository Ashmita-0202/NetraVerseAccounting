using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddTransactionDetails
{
    public record AddTransactionItemsRequest
    (
            string? id,
            decimal amount,
            string? remarks,
            string ledgerId
  
    );
}
