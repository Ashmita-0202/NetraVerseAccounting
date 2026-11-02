
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddExpense
{
    public record AddExpenseResponse
    (
            string id = "",
            string transactionDate = default,
            decimal totalAmount = 0,
            string narration = "",
            TransactionType transactionMode = default,
             string paymentMethodId="",
             string transactionNumber = "",
            List<AddTransactionItemsRequest> TransactionsItems = null!
        );
    
}
