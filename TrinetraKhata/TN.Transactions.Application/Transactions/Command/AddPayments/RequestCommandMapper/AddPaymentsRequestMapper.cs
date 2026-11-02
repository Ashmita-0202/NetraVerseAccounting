using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Transactions.Application.Transactions.Command.AddReceipt;

namespace TN.Transactions.Application.Transactions.Command.AddPayments.RequestCommandMapper
{
    public static class AddPaymentsRequestMapper
    {
        public static AddPaymentsCommand ToCommand(this AddPaymentsRequest request)
        {
            return new AddPaymentsCommand(
                 request.transactionDate,
                    request.totalAmount,
                    request.narration,
                    request.transactionMode,
                    request.paymentsNumber,
                    request.paymentMethodId,
                    request.chequeNumber,
                    request.bankName,
                    request.accountName,
                    request.addTransactionItemsForPayments
                );
        }
    }
}
