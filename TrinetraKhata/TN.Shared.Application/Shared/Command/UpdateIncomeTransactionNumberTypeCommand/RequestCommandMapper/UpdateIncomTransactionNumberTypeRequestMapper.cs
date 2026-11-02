using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand.RequestCommandMapper
{
    public static class UpdateIncomTransactionNumberTypeRequestMapper
    {
        public static UpdateIncomeTransactionNumberTypeCommand ToCommand(this UpdateIncomeTransactionNumberTypeRequest request,string companyId)
        {
            return new UpdateIncomeTransactionNumberTypeCommand(
            request.transactionNumberType,
            companyId
            );
        }
    }
}
