using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;

namespace TN.Transactions.Application.Transactions.Command.UpdateTransactions
{
    public record  UpdateTransactionsResponse
    (
            string id,
            string? transactionDate,   
            decimal? totalAmount,
            string? narration,
            List<UpdateTransactionDetailsRequest> transactionsDetails
    );
    public record UpdateTransactionDetailsRequest
        (

             decimal? amount,
             string remarks,
             string ledgerId



        );
        
        
}
