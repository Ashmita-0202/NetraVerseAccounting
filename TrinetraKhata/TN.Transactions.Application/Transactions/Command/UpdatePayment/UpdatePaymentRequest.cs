
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdatePayment
{
    public record  UpdatePaymentRequest
    (

            string transactionDate = "",
            decimal totalAmount = 0,
            string narration = "",
            TransactionType transactionMode = default,
            string? paymentsNumber = null,
            string? paymentMethodId = "",
            string? chequeNumber="",
            string? bankName="",
            string? accountName="",
            List<UpdateTransactionItemRequest> addTransactionItemsForPayments = null!
    );
}
