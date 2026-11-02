using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate
{
    public record class GetFilterIncomeByDateQueryResponse
    (
           string id = "",
            string? transactionDate = default,
            decimal totalAmount = 0,
            string? narration = "",
            TransactionType transactionMode = default,
            List<AddTransactionItemsRequest> addTransactionItemsForIncome = null!

    );
}
