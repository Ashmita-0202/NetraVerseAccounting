
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddReceipt
{
    public record AddReceiptRequest
    (
             string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType transactionMode,
                string? receiptNumber,
            string paymentMethodId,
              string? chequeNumber,
  string? bankName,
  string? accountName,
            List<AddTransactionItemsForReceipt> transactionItemsForReceipts
        );
    
}
