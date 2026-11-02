
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.GetPaymentById
{
    public record  GetPaymentByIdQueryResponse
    (
            string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
            string? paymentMethodId = "",
            string? paymentsNumber = "",
            string? chequeNumber=null,
            string? bankName=null,
            string? accountName=null,
            List<UpdateTransactionItemRequest> addTransactionItemsForPayments = null!

    );
}
