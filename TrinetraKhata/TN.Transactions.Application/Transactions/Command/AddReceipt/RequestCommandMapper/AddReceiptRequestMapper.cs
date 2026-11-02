using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddReceipt.RequestCommandMapper
{
    public static class AddReceiptRequestMapper
    {
        public static AddReceiptCommand ToCommand(this AddReceiptRequest request)
        {
            return new AddReceiptCommand(
                 request.transactionDate,
                    request.totalAmount,
                    request.narration,
                    request.transactionMode,
                    request.receiptNumber,
                    request.paymentMethodId,
                    request.chequeNumber,
                    request.bankName,
                    request.accountName,
                    request.transactionItemsForReceipts
                );
        }
    }
}
