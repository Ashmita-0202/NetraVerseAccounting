
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.GetAllExpense
{
    public record  GetAllExpenseQueryResponse
    (
            string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType TransactionMode = default,
            List<AddTransactionItemsRequest> addTransactionsItemsForExpense = null!
    );
}
