
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdateExpense
{
    public record UpdateExpenseRequest
    (
          string? transactionDate = "",
          decimal totalAmount = 0,
          string? narration = "",
          TransactionType transactionMode = default,
          string? expensesNumber = null,
            string? paymentMethodId = "",
            string? chequeNumber = "",
            string? bankName = "",
            string? accountName = "",
          List<UpdateTransactionItemRequest> addTransactionsItemsForExpense = null!
    );
}
