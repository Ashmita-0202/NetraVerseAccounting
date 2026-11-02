
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdateIncome
{
    public record  UpdateIncomeCommand
    (
            string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
            string? paymentId="",
            string? incomeNumbers=null,
            string? chequeNumber=null,
            string? bankName=null,
            string? accountName=null,
              List<UpdateTransactionItemRequest> TransactionsItems = null!
    ) :IRequest<Result<UpdateIncomeResponse>>;
}
