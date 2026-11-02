using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddExpense
{
    public record AddExpenseCommand
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
        ) : IRequest<Result<AddExpenseResponse>>;
    
}
