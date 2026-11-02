using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.UpdatePayment.RequestCommandMapper
{
    public static class  UpdatePaymentRequestMapper
    {
        public static UpdatePaymentCommand ToUpdatePaymentCommand(this UpdatePaymentRequest request,string id)
        {
            return new UpdatePaymentCommand
            (
                id,
                request.transactionDate,
                request.totalAmount,
                request.narration,
                request.transactionMode,
                request.paymentsNumber,
                request.paymentMethodId           ,request.chequeNumber,
                request.bankName,
                request.accountName,
                request.addTransactionItemsForPayments

            );
        }
    }
}
