using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace NV.Payment.Domain.Entities
{
    public class PaymentMethod : Entity
    {
        public PaymentMethod(
            ): base(null)
        {
            
        }

        public PaymentMethod(
            string id,
            PaymentType type,
            string provider,
            decimal processingFee
            ): base(id)
        {
            Type = type;
            Provider = provider;
            ProcessingFee = processingFee;
            Payments = new List<Payments>();
            
        }
        #region API based 
        //public string Provider {  get; set; }
        //public string ApiKey { get; set; }
        //public decimal ProcessingFee { get; set; }

        #endregion

        public string Name { get; set; }

        public PaymentType Type { get; set; }

        public string? Provider { get; set; } //Eg. XYZ Bank, Stripe

        public decimal? ProcessingFee { get; set; } //For Deigtal Payment only

        public virtual ICollection<Payments> Payments { get; set; }
        public enum PaymentType
        {
            Cash=1,
            BankTransfer=2,
            CreditCard=3,
            OnlineWallet=4
        }
    }
}
