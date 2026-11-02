using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
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
            string? fyId,
             List<JournalEntryDetails> journalEntryDetails,
             bool isActive
            ) : base(id)
        {
            IsActive = isActive;
            ReferenceNumber = referenceNumber;
            TransactionDate = transactionDate;
            Description = description;
            CreatedBy = createdBy;
            CompanyId = companyId;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            BillNumbers = billNumber;
            FyId = fyId;
            JournalEntryDetails= journalEntryDetails;
            PurchaseDetails = new List<PurchaseDetails>();
            SalesDetails = new List<SalesDetails>();


        }

        public bool IsActive { get; set; }
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

        public string? FyId { get;set; }
        //Navigation Property
        public ICollection<JournalEntryDetails> JournalEntryDetails { get; set; }

        public ICollection<PurchaseDetails> PurchaseDetails { get; set; }
        public ICollection<SalesDetails> SalesDetails { get; set; }


    }
}
