
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddPayments
{
    public record AddPaymentsResponse
    (
        string id = "",
            DateTime? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
            string transactionNumber="",
            List<AddTransactionItemsRequest> TransactionsItems = null!
        );
    
}
