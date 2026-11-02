
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.GetAllReceipt
{
    public record  GetAllReceiptQueryResponse
    (
            string id = "",
            string? transactionDate = default,
            decimal? totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
              string? chequeNumber = "",
            string? bankName = "",

            List<AddTransactionItemsRequest> transactionItemsForReceipts = null!

    );
}
