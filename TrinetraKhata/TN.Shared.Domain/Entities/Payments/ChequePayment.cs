using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Shared.Domain.Entities.Payments
{
    public class ChequePayment : PaymentsDetails
    {
     
        public ChequePayment(
            string id,
            TransactionType transactionType,
            DateTime? transactionDate,
            decimal totalAmount,
            string transactionDetailsId,
            string paymentMethodId,
            string? chequeNumber,
            string? bankName,
            string? accountName,
            string? companyId,
            DateTime? chequeDate
            ) : base(id, transactionType, transactionDate, totalAmount, transactionDetailsId, paymentMethodId, companyId)
        {
            ChequeNumber = chequeNumber;
            BankName = bankName;
            AccountName = accountName;
            ChequeDate = chequeDate;

        }

        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountName { get; set; }
        public DateTime? ChequeDate { get; set; }

        }
}
