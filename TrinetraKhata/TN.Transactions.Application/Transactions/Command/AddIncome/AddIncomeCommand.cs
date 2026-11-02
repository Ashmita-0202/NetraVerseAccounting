using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.Transactions.Command.AddPayments;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Application.Transactions.Command.AddIncome
{
    public record AddIncomeCommand
    (
            string? transactionDate,
            decimal totalAmount,
            string? narration,
            TransactionType TransactionMode,
            string? incomeNumber,
            string paymentMethodId,
            string? chequeNumber,
            string? bankName,
            string? accountName,
            List<AddTransactionItemsForIncome> addTransactionItemsForIncome
        ) : IRequest<Result<AddIncomeResponse>>;
}
