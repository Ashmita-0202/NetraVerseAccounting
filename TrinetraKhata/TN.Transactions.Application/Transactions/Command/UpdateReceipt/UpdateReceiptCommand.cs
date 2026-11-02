
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt
{
    public record  UpdateReceiptCommand
    (
            string id = "",
            string transactionDate = "",
            decimal totalAmount = 0,
            string narration = "",
            TransactionType transactionMode = default,
            string? receiptNumber=null,
            string? paymentId= null,
            string? chequeNumber=null,
            string? bankName=null,
            string? accountName=null,
            List<UpdateTransactionItemRequest> TransactionsItems = null!

    ):IRequest<Result<UpdateReceiptResponse>>;
}
