using NV.Payment.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public class Ledger : Entity
    {
        public Ledger(
            string id,
            string name,
            string createdDate,
            bool? isInventoryAffected,
            string? address,
            string? panNo,
            string? phoneNumber,
            string? maxCreditPeriod,
            string? maxDuePeriod,
            string ledgerGroupId,
            string companyId
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
            LedgerGroupId = ledgerGroupId;
            CompanyId = companyId;
            Customers = new List<Customers>();
            JournalEntryDetails = new List<JournalEntryDetails>();
            Payments = new List<Payments>();    

        }
        public string Name { get; set; }
        public string CreatedDate {  get; set; }
        public bool? IsInventoryAffected { get; set;}
        public string? Address { get; set; }
        public string? PanNo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MaxCreditPeriod { get; set; }
        public string? MaxDuePeriod { get; set; }
        public string LedgerGroupId {  get; set; }
        public string CompanyId { get; set; }

        //Navigation Properties
        public LedgerGroup LedgerGroups { get; set; }
        public ICollection<Customers> Customers { get; set; }
        public virtual ICollection<JournalEntryDetails> JournalEntryDetails { get; set; }

        public virtual ICollection<Payments> Payments { get; set; }
  
    }
}
