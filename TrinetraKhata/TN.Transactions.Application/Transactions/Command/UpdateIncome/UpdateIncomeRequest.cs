
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.UpdateIncome
{
    public record  UpdateIncomeRequest
    (
            string? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
            string? paymentMethodId = "",
            string? incomeNumber = null,
            string? chequeNumber="",
            string? bankName="",
            string? accountName="",
            List<UpdateTransactionItemRequest> addTransactionItemsForIncome = null!
    );
}
