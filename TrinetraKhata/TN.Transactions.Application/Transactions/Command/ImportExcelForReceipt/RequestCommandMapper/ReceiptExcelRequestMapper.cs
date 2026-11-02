using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt.RequestCommandMapper
{
    public static class ReceiptExcelRequestMapper
    {
        public static ReceiptExceCommand ToCommand(this ReceiptExcelRequest request)
        {
            return new ReceiptExceCommand
                (
                     request.formFile
                );
        }
    }
}
