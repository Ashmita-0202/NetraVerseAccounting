using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.UpdateIncome.RequestCommandMapper
{
    public static class UpdateIncomeRequestMapper
    {
        public static UpdateIncomeCommand ToCommand(this UpdateIncomeRequest request,string id) 
        {
            return new UpdateIncomeCommand
            (
                id,
                request.transactionDate,
                request.totalAmount,
                request.narration,
                request.transactionMode,
                request.paymentMethodId,
                request.incomeNumber,
                request.chequeNumber,
                request.bankName,
                request.accountName,
                request.addTransactionItemsForIncome);

        }
    }
}
