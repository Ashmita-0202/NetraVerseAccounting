


using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddExpense
{
    public record AddExpenseRequest
    (
                       string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType transactionMode,
            string? expensesNumber,
           string paymentMethodId,
                       string? chequeNumber,
string? bankName,
string? accountName,
            List<AddTransactionsItemsForExpense> addTransactionsItemsForExpense
        );
}
