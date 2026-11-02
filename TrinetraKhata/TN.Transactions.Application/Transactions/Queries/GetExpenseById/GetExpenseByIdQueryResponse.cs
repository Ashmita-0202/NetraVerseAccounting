
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.GetExpenseById
{
    public record  GetExpenseByIdQueryResponse
    (

           string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
            string? paymentMethodId = null,
            string? expensesNumber = null,
            string? chequeNumber=null,
            string? bankName=null,
            string? accountName = null,
            List<UpdateTransactionItemRequest> addTransactionsItemsForExpense = null!
    );
}
