using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType.RequestCommandMapper
{
    public static class UpdateExpenseTransactionNumberTypeRequestMaper
    {
        public static UpdateExpenseTransactionNumberTypeCommand ToCommand(this UpdateExpenseTransactionNumberTypeRequest request, string companyId)
        {
            return new UpdateExpenseTransactionNumberTypeCommand(
                request.transactionNumberType,
                companyId       
            );
        }
    }
}
