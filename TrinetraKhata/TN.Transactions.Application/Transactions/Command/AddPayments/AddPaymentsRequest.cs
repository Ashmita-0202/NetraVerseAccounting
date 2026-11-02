

using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddPayments
{
    public record AddPaymentsRequest
    (
            string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType transactionMode,
            string? paymentsNumber,
            string paymentMethodId,
            string? chequeNumber,
            string? bankName,
            string? accountName,
            List<AddTransactionItemsForPayments>  addTransactionItemsForPayments
        );
    
}
