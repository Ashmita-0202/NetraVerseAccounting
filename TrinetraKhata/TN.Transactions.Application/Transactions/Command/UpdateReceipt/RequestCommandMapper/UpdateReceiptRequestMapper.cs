using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt.RequestCommandMapper
{
    public static class UpdateReceiptRequestMapper
    {
        public static UpdateReceiptCommand ToCommand(this UpdateReceiptRequest request, string id) 
        {
            return new UpdateReceiptCommand
                (
                     id,
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
