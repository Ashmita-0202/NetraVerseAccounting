using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddIncome.RequestCommandMapper
{
    public static class AddIncomeRequestMapper
    {
        public static AddIncomeCommand ToCommand(this AddIncomeRequest request)
        {
            return new AddIncomeCommand(
                    request.transactionDate,
                    request.totalAmount,
                    request.narration,
                    request.transactionMode,
                    request.incomeNumber,
                    request.paymentMethodId,
                    request.chequeNumber,
                    request.bankName,
                    request.accountName,
                    request.addTransactionItemsForIncome
                );
        }
    }
}
