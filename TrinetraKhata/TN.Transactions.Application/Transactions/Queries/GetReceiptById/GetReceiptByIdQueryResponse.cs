
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using TN.Transactions.Application.Transactions.Queries.ReceiptVouchers;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.GetReceiptById
{
    public record GetReceiptByIdQueryResponse
    (

            string id = "",
            string? transactionDate = default,
            decimal? totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
            string? receiptNumber = null,
            string? paymentMethodId=null,
             string? chequeNumber = null,
            string? bankName = null,
            string? accountName = null,
            List<UpdateTransactionItemRequest> transactionItemsForReceipts = null!
    );
}
