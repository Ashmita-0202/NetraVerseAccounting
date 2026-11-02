using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Account
{
    public class SubLedgerGroup : Entity
    {

        public SubLedgerGroup(
            string id,
            string name,
            string ledgerGroupId,
            string companyId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool? isSeeded,
            bool isActive
            ) : base(id)
        {
            Name = name;
            LedgerGroupId = ledgerGroupId;
            CompanyId = companyId;   
            CreatedBy = createdBy;   
            CreatedAt = createdAt;   
            ModifiedBy = modifiedBy; 
            ModifiedAt = modifiedAt;
            IsSeeded = isSeeded;
            IsActive = isActive;
            Ledgers = new List<Ledger>();
            PaymentMethods = new List<PaymentMethod>();
        }

        public string Name { get; set; }
        public string LedgerGroupId { get; set; }
        public LedgerGroup? LedgerGroup { get; set; }
        public ICollection<Ledger> Ledgers { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; } 
        public string CompanyId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public bool? IsSeeded { get; set; }
        public bool IsActive { get; set; }
    }
}
