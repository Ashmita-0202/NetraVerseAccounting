
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.GetIncomeById
{
    public record  GetIncomeByIdQueryResponse
    (
           string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
            string? paymentMethodId = null,
            string? incomeNumber = null,
                                                 string? chequeNumber=null,
string? bankName=null,
string? accountName=null,
            List<UpdateTransactionItemRequest> addTransactionItemsForIncome = null!

    );
}
