using Microsoft.Identity.Client;
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
    public class LedgerGroup : Entity
    {

        public LedgerGroup() : base(null)
        {

        }

        //public LedgerGroup(string name, bool isCustom, int masterId, bool isPrimary)
        //{
        //    Name = name;
        //    IsCustom = isCustom;
        //    MasterId = masterId;
        //    IsPrimary = isPrimary;
        //}

        public LedgerGroup(
            string id,
            string name,
            bool isCustom,
            string masterId,
            bool isPrimary,
            string companyId,
            string fiscalId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool? isSeeded,
            bool isActive


            )
            : base(id)
        {
            Name = name;
            IsCustom = isCustom;
            MasterId = masterId;
            IsPrimary = isPrimary;
            CompanyId = companyId;
            FiscalId = fiscalId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            IsSeeded = isSeeded;
            IsActive = isActive;  
            Ledgers = new List<Ledger>();
            PaymentMethods = new List<PaymentMethod>();
            SubLedgerGroups = new List<SubLedgerGroup>();

        }
        public string Name { get; set; }
        public bool IsCustom { get; set; }
        public string MasterId { get; set; }
        public Master Masters { get; set; }
        public bool IsPrimary { get; set; }
        public string CompanyId { get; set; }

        public string FiscalId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool? IsSeeded { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Ledger> Ledgers { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }
        public ICollection<SubLedgerGroup> SubLedgerGroups { get; set; }
    }
}
