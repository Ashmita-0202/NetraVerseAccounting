
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddIncome
{
    public record AddIncomeResponse
    (
         string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string narration = "",
            string paymentMethodId="",
            string transactionNumber = "",
            TransactionType transactionMode = default,
            List<AddTransactionItemsRequest> TransactionsItems = null!
        );
    
}
