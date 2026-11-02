using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Queries.ReceiptVouchers;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.Transactions.Application.Transactions.Queries.GetTransactionsById
{
    public record  GetTransactionsByIdQueryResponse
    (
             string id,
            string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType transactionMode,
            List<ReceiptVouchersRequest> transactionsItems

    );
}
