using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Transactions;
using TN.Transactions.Application.Transactions.Queries.ReceiptVouchers;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.Transactions.Application.Transactions.Queries.GetAllTransactions
{
    public record  GetAllTransactionsByQueryResponse
    (
              string id,
            string? transactionDate,
            decimal? totalAmount,
            string? narration,
            TransactionType transactionMode,
             string paymentId,
            List<ReceiptVouchersRequest> transactionsItems

    );

}
