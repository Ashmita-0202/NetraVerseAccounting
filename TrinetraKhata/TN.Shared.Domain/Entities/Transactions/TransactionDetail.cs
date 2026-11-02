using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Transactions
{
    public class TransactionDetail : Entity
    {
        public TransactionDetail(
            ) : base(null)
        {

        }

        public TransactionDetail(
            string id,
            string? transactionDate,
            decimal totalAmount,
            string? narration,
            string companyId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            TransactionType transactionMode,
            string paymentMethodId,
            string? journalEntriesId,
            string? transactionNumber,
            List<TransactionItems> transactionItems

            ) : base(id)
        {
            TransactionDate = transactionDate;
            TotalAmount = totalAmount;
            Narration = narration;
            CompanyId = companyId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            TransactionMode = transactionMode;
            PaymentMethodId = paymentMethodId;
            TransactionNumber = transactionNumber;
            TransactionsItems = transactionItems;
            JournalEntriesId = journalEntriesId;

        }
        public string? TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Narration { get; set; }
        public string CompanyId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public TransactionType TransactionMode { get; set; }

        public string PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public string? TransactionNumber { get; set; }

        public string? JournalEntriesId { get; set; }

        public ICollection<TransactionItems> TransactionsItems { get; set; }
        public enum TransactionType
        {

            Income = 1,
            Expense = 2,
            Receipts = 3,
            Payment = 4,
            Purchase = 5,
            Sales = 6,
            PurchaseReturn = 7,
            SalesReturn = 8
        }

    }
}
