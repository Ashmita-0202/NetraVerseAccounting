using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Transactions
{
    public class TransactionItems: Entity
    {

        public TransactionItems(
            ): base(null)
        {
            
        }

        public TransactionItems(
            string id,
            decimal amount,
            string? remarks,
            string transactionDetailId,
            string ledgerId
            ): base(id)
        {
            Amount = amount;
            Remarks = remarks;
            TransactionDetailId = transactionDetailId;
            LedgerId = ledgerId;

            
        }

        public decimal Amount { get; set; }
        public string? Remarks { get; set; }
        public string LedgerId { get; set; }
        public virtual Ledger Ledgers { get; set; }
        public string TransactionDetailId { get; set; }
        public virtual TransactionDetail TransactionDetail { get; set; }
    }
}
