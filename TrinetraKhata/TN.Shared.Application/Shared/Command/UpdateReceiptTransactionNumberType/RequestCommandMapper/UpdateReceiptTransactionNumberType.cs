using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType.RequestCommandMapper
{
    public static class UpdateReceiptTransactionNumberType
    {
        public static UpdateReceiptTransactionNumberTypeCommand ToCommand(this UpdateReceiptTransactionNumberTypeRequest request, string companyId)
        {
            return new UpdateReceiptTransactionNumberTypeCommand
           (
                
                request.transactionNumberType,
                companyId
            );
        }
       
    }
}
