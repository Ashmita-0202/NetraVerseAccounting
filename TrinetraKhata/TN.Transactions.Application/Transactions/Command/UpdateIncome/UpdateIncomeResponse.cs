
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdateIncome
{
    public record  UpdateIncomeResponse
    (
         
          string? transactionDate = default,
          decimal totalAmount = 0,
          string? Narration = "",
          TransactionType transactionMode = default,
          List<UpdateTransactionItemRequest> TransactionsItems = null!
    );
}
