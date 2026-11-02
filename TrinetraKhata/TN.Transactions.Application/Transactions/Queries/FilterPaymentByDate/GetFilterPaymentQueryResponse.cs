
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.FilterPaymentByDate
{
    public record  GetFilterPaymentQueryResponse
    (

            string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string narration = "",
            TransactionType transactionMode = default,
            List<AddTransactionItemsRequest> addTransactionItemsForPayments = null!
    );
}
