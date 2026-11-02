using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;

namespace TN.Transactions.Application.Transactions.Command.UpdateTransactions
{
    public record  UpdateTransactionsCommand
    (
            string id,
            DateTime transactionDate,
            decimal totalAmount,
            string narration,
             List<UpdateTransactionDetailsDto> updateTransactionsDetails


    ) :IRequest<Result<UpdateTransactionsResponse>>;
}
