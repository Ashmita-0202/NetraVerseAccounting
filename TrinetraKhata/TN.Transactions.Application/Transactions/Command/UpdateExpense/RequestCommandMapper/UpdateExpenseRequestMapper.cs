using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.UpdateExpense.RequestCommandMapper
{
    public static class UpdateExpenseRequestMapper
    {
        public static UpdateExpenseCommand ToCommand(this UpdateExpenseRequest request, string id)
        {
            return new UpdateExpenseCommand
            (
                id,
                request.transactionDate,
                request.totalAmount,
                request.narration,
                request.transactionMode,
                request.paymentMethodId,
                request.chequeNumber,
                request.bankName,
                request.accountName,
                request.addTransactionsItemsForExpense
            );
        }
    }
}
