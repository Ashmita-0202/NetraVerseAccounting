
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdateExpense
{
    public record  UpdateExpenseCommand
    (
          string id = "",
          string? transactionDate = default,
          decimal totalAmount = 0,
          string? narration = "",
          TransactionType transactionMode = default,
          string? paymentId="",
            string? chequeNumber="",
            string? bankName="",
            string? accountName="",
          List<UpdateTransactionItemRequest> TransactionsItems = null!

    ):IRequest<Result<UpdateExpenseResponse>>;
}
