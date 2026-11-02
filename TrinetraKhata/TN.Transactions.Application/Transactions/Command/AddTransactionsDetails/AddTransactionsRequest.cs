using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Transactions;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddTransactions
{
    public record AddTransactionsRequest
    (
             
            string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType transactionMode,
            string paymentId,
            List<AddTransactionItemsRequest> transactionsItems
   );
}
