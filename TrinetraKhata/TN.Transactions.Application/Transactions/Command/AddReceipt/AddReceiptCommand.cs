
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddReceipt
{
    public record AddReceiptCommand
    (
        string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType transactionMode,
            string? receiptNumber,
            string paymentId,
              string? chequeNumber,
  string? bankName,
  string? accountName,
            List<AddTransactionItemsForReceipt> transactionItemsForReceipts
        ) : IRequest<Result<AddReceiptResponse>>;
}
