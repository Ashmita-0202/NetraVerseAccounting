
using TN.Shared.Domain.Primitive;

namespace NV.Payment.Domain.Entities
{
    public class Payments : Entity
    {
        public Payments(): base(null)
        {
            
        }

        public Payments(
            string id,
            string paymentMethod,
            decimal amount,
            DateTime paymentDate,
            string ledgerId,
            string debitLedgerId,
            string creditLedgerId
            ) : base(id)
        {
            
        }

        public string PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string LedgerId { get; set; }

        // Double-entry accounting: Store Debit & Credit Ledgers
        public string DebitLedgerId { get; set; }
        //public Ledger DebitLedger { get; set; }
        public string CreditLedgerId { get; set; }
        //public Ledger CreditLedger { get; set; }
    }
}
