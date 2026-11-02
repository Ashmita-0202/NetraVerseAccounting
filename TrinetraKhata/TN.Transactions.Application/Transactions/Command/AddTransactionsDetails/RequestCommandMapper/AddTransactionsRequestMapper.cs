using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddTransactions.RequestCommandMapper
{
    public static  class AddTransactionsRequestMapper
    {
        public static AddTransactionsCommand ToCommand(this AddTransactionsRequest request) 
        {
            return new AddTransactionsCommand
                (
                    request.transactionDate,
                    request.totalAmount,
                    request.narration,
                    request.transactionMode,
                    request.paymentId,
                    request.transactionsItems
                
                );
        
        }
    }
}
