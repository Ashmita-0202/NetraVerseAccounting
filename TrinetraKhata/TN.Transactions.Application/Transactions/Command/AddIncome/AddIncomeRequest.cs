


using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddIncome
{
    public record AddIncomeRequest
    (
        string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType transactionMode,
                string? incomeNumber,
            string paymentMethodId,
            string? chequeNumber,
             string? bankName,
           string? accountName,
            List<AddTransactionItemsForIncome> addTransactionItemsForIncome
        );
}
