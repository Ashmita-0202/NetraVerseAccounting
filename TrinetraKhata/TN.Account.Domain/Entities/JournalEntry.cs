using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public class JournalEntry : Entity
    {
        public JournalEntry(): base(null)
        {
        }

        public JournalEntry(
            string id,
            string referenceNumber,
            DateTime transactionDate,
            string description,
            string createdBy,
            string companyId,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string? billNumber,
             List<JournalEntryDetails> journalEntryDetails
            ) : base(id)
        {
            ReferenceNumber = referenceNumber;
            TransactionDate = transactionDate;
            Description = description;
            CreatedBy = createdBy;
            CompanyId = companyId;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            BillNumbers = billNumber;
            JournalEntryDetails= journalEntryDetails;
        }
        [Required]
        [MaxLength(100)]
        public string ReferenceNumber { get;set; }
        [Required]
        public DateTime TransactionDate { get;set; }
        public string Description { get;set; }
        [Required]
        public string CreatedBy { get;set;} 
        public DateTime CreatedAt { get;set;}= DateTime.UtcNow;
        public string CompanyId { get;set; }
        public string ModifiedBy { get;set;}
        public DateTime ModifiedAt { get;set;}
        public string? BillNumbers { get;set; }
        //Navigation Property
        public ICollection<JournalEntryDetails> JournalEntryDetails { get; set; }


    }
}
