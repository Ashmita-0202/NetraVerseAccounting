
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.GetAllPayments
{
    public record  GetAllPaymentsQueryResposne
    (
            string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType TransactionMode = default,
              string? chequeNumber = "",
            string? bankName = "",
            List<AddTransactionItemsRequest> addTransactionItemsForPayments = null!
    );
}
