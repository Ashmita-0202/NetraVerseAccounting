
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdateExpense
{
    public record UpdateExpenseResponse
    (
          string id = "",
          string? transactionDate = "",
          decimal totalAmount = 0,
          string? narration = "",
          TransactionType TransactionMode = default,
          List<UpdateTransactionItemRequest> TransactionsItems = null!  
    );
}
