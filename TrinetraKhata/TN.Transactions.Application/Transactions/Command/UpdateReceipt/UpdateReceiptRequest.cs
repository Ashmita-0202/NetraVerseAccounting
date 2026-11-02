
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt
{
    public record  UpdateReceiptRequest
    (

            string transactionDate = "",
            decimal totalAmount = 0,
            string narration = "",
            TransactionType transactionMode = default,
            string? receiptNumber = null,
            string? paymentMethodId = "",
            string? chequeNumber="",
            string? bankName="",
            string? accountName="",
            List<UpdateTransactionItemRequest> transactionItemsForReceipts = null!

    );
}
