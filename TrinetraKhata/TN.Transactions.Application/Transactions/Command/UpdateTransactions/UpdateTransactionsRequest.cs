using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;

namespace TN.Transactions.Application.Transactions.Command.UpdateTransactions
{
    public record UpdateTransactionsRequest
    (

            DateTime transactionDate,
            decimal totalAmount,
            string narration,
            List<UpdateTransactionDetailsDto> transactionsDetails

    );
    public record UpdateTransactionDetailsDto
        (
             string amount,
            string remarks,
            string transactionId,
            string ledgerId

        );
}
