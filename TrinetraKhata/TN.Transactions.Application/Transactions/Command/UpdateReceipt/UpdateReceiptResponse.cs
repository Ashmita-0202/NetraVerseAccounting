using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt
{
    public record  UpdateReceiptResponse
    (
          
            string transactionDate = "",
            decimal totalAmount = 0,
            string narration = "",
            TransactionType transactionMode = default,
            List<UpdateTransactionItemRequest> TransactionsItems = null!
    );
}
