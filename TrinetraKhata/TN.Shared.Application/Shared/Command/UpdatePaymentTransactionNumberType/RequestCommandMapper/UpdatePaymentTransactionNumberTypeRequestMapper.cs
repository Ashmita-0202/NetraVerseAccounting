using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType.RequestCommandMapper
{
    public static class UpdatePaymentTransactionNumberTypeRequestMapper
    {
        public static UpdatePaymentTransactionNumberTypeCommand ToCommand(this UpdatePaymentTransactionNumberTypeRequest request, string companyId)
        {
            return new UpdatePaymentTransactionNumberTypeCommand
            (
                request.transactionNumberType,
                companyId
            );
        }

        //public static UpdatePaymentTransactionNumberTypeResponse ToResponse(this UpdatePaymentTransactionNumberTypeCommand command)
        //{
        //    return new UpdatePaymentTransactionNumberTypeResponse
        //    {
        //        TransactionNumberType = command.TransactionNumberType,
        //        CompanyId = command.CompanyId
        //    };
        //}
    }
}
