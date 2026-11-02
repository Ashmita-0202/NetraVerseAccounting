using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Shared.Domain.Entities.Payments
{
    public class PaymentsDetails : Entity
    {
        public PaymentsDetails(
            ): base(null)
        {
            
        }

        public PaymentsDetails(
            string id,
            TransactionType transactionType,
            DateTime? transactionDate,
            decimal totalAmount,
            string transactionDetailsId,
            string paymentMethodId,
            string? companyId
            ) : base(id)
        {
            TransactionType = transactionType;
            TransactionDate = transactionDate;
            TotalAmount = totalAmount;
            TransactionDetailsId = transactionDetailsId;
            PaymentMethodId = paymentMethodId;
            CompanyId = companyId;

        }
        public TransactionType TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string TransactionDetailsId { get; set; }
        public string? CompanyId { get; set; }  
        public string PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

    }
}
