
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TN.Shared.Domain.Primitive;
namespace TN.Account.Domain.Entities
{
    public class JournalEntryDetails: Entity
    {
        public JournalEntryDetails(
            ): base(null)
        {
            

        }

   

        public JournalEntryDetails(
            string id,
            string journalEntryId,
            string ledgerId,
            decimal debitAmount,
            decimal creditAmount,
            DateTime transactionDate,
            string companyId,
            string? fiscalId,
            bool isActive
            ): base(id) 

        {
            IsActive = isActive;
            JournalEntryId = journalEntryId;
            LedgerId = ledgerId;
            DebitAmount = debitAmount;
            CreditAmount = creditAmount;
            TransactionDate = transactionDate;
            CompanyId = companyId;
            FiscalId = fiscalId;
            
        }

        public bool IsActive { get; set; }
        [Required]
        public string JournalEntryId { get; set; }
        [Required]
        public string LedgerId { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal DebitAmount { get; set;}
        [Column(TypeName ="decimal(18,2)")]
        public decimal CreditAmount { get;set;}

        public DateTime TransactionDate {  get; set; }
        public string CompanyId { get; set; }

        //Navigation Property
        public  JournalEntry JournalEntry { get; set; }
        public  Ledger Ledger { get; set; }
        public string? FiscalId { get; set; }
    }
}
