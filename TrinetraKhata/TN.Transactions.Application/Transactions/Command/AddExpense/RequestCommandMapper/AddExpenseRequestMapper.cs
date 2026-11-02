using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddExpense.RequestCommandMapper
{
    public static class AddExpenseRequestMapper
    {
        public static AddExpenseCommand ToCommand(this AddExpenseRequest request)
        {
            return new AddExpenseCommand(
                    request.transactionDate,
request.totalAmount,
request.narration,
request.transactionMode,
request.expensesNumber,
request.paymentMethodId,
request.chequeNumber,
request.bankName,
request.accountName,
request.addTransactionsItemsForExpense
                );
        }
    }
}
