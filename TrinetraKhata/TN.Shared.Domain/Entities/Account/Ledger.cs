using NV.Payment.Domain.Entities;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.Transactions;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public class Ledger : Entity
    {
        public Ledger(
            string id,
            string name,
            DateTime createdDate,
            bool? isInventoryAffected,
            string? address,
            string? panNo,
            string? phoneNumber,
            string? maxCreditPeriod,
            string? maxDuePeriod,
            string subLedgerGroupId,
            string companyId,
            string? fyId,
            decimal? openingBalance,
            bool? isSeeded,
            bool isActive
            ) : base(id) 
        { 

            Name = name;
            CreatedDate = createdDate;
            IsInventoryAffected = isInventoryAffected;
            Address = address;
            PanNo = panNo;
            PhoneNumber = phoneNumber;
            MaxCreditPeriod = maxCreditPeriod;
            MaxDuePeriod = maxDuePeriod;
            SubLedgerGroupId = subLedgerGroupId;
            CompanyId = companyId;
            FyId= fyId;
            OpeningBalance = openingBalance;
            IsActive = isActive;
    
            IsSeeded = isSeeded;
            Customers = new List<Customers>();
            JournalEntryDetails = new List<JournalEntryDetails>();
            Payments = new List<Payments>();
            TransactionItems = new List<TransactionItems>();
            OpeningBalances = new List<OpeningBalance>();
            ClosingBalances = new List<ClosingBalance>();

        }

        public bool IsActive { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate {  get; set; }
        public bool? IsInventoryAffected { get; set;}
        public string? Address { get; set; }
        public string? PanNo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MaxCreditPeriod { get; set; }
        public string? MaxDuePeriod { get; set; }
        public string SubLedgerGroupId {  get; set; }
        public string CompanyId { get; set; }

        public string? FyId { get; set; }
        public decimal? OpeningBalance { get; set; }
        public bool? IsSeeded { get; set; } = true;

        //Navigation Properties
        public virtual SubLedgerGroup SubLedgerGroup { get; set; }
        public ICollection<Customers> Customers { get; set; }
        public virtual ICollection<JournalEntryDetails> JournalEntryDetails { get; set; }

        public virtual ICollection<Payments> Payments { get; set; }

        public virtual ICollection<TransactionItems> TransactionItems { get; set; }

        public virtual ICollection<OpeningBalance> OpeningBalances { get; set; }
        public virtual ICollection<ClosingBalance> ClosingBalances { get; set; }


  
    }
}
