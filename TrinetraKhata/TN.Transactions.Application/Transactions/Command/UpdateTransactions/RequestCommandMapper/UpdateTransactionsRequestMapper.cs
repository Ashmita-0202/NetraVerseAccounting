using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.UpdateTransactions.RequestCommandMapper
{
    public static class UpdateTransactionsRequestMapper
    {
        public static UpdateTransactionsCommand ToCommand(this UpdateTransactionsRequest request, string id) 
        {
            return new UpdateTransactionsCommand
                (
                
                     id,
                     request.transactionDate,
                     request.totalAmount,
                     request.narration,
                     request.transactionsDetails

                );
        
        }
    }
}
