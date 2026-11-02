using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.Transactions;
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
            string name,
            string subLedgerGroupsId,
            //PaymentType type,
            string companyId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool? isChequeNo,
            bool? isBankName,
            bool? isAccountName,
            bool? isChequeDate,
            bool? isCardNumber,
            bool? isCardHolderName,
            bool? isExpiryDate

            ) : base(id)
        {
            Name = name;
            SubLedgerGroupsId = subLedgerGroupsId;
            //Type = type;
            CompanyId = companyId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            IsChequeNo = isChequeNo;
            IsBankName = isBankName;
            IsAccountName = isAccountName;
            IsChequeDate = isChequeDate;
            IsCardNumber = isCardNumber;
            IsCardHolderName = isCardHolderName;
            IsExpiryDate = isExpiryDate;
            Payments = new List<Payments>();
            TransactionDetails = new List<TransactionDetail>();
            PaymentsDetails = new List<PaymentsDetails>();  


        }
        #region API based 
        //public string Provider {  get; set; }
        //public string ApiKey { get; set; }
        //public decimal ProcessingFee { get; set; }

        #endregion

        public string CompanyId { get; set; }
        public string Name { get; set; }

        public string SubLedgerGroupsId {  get; set; }

        public virtual SubLedgerGroup SubLedgerGroups { get; set; }
        //public PaymentType Type { get; set; }

        //public string? Provider { get; set; } //Eg. XYZ Bank, Stripe

        //public decimal? ProcessingFee { get; set; } //For Deigtal Payment only

        public virtual ICollection<Payments> Payments { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
        public virtual ICollection<PaymentsDetails> PaymentsDetails { get; set; }
        //public enum PaymentType
        //{
        //    Cash=1,
        //    BankTransfer=2,
        //    CreditCard=3,
        //    OnlineWallet=4
        //}

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public bool? IsChequeNo { get; set; }
        public bool? IsBankName { get; set; }    
        public bool? IsAccountName { get; set; }
        public bool? IsChequeDate { get; set; }
        public bool? IsCardNumber { get; set; }  
        public bool? IsCardHolderName { get; set; }
        public bool? IsExpiryDate { get; set; }
    }
}
