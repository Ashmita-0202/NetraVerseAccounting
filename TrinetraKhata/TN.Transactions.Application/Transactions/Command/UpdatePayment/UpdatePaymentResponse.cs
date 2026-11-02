
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdatePayment
{
    public record  UpdatePaymentResponse
    (
           
            string transactionDate = "",
            decimal totalAmount = 0,
            string narration = "",
            TransactionType transactionMode = default,
            List<UpdateTransactionItemRequest> TransactionsItems = null!
    );
}
