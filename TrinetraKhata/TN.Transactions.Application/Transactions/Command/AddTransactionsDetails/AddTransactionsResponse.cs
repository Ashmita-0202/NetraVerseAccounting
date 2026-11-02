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
    public record  AddTransactionsResponse
    (
            string id="",
            DateTime? transactionDate=default,
            decimal totalAmount=0,
            string? narration="",
            TransactionType transactionMode=default,
            List<AddTransactionItemsRequest> TransactionsItems = null!
    );
}
