using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
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
            string companyId
            
            )
            :base(id)
        {
            Name = name;
            IsCustom = isCustom;
            MasterId = masterId;
            IsPrimary = isPrimary;
            CompanyId = companyId;
            Ledgers = new List<Ledger>();


        }
        public string Name { get;set; }
        public bool IsCustom { get;set; }
        public string MasterId {  get;set; }
        public Master Masters { get;set; }
        public bool IsPrimary { get;set; }
        public string CompanyId { get; set; }

        public ICollection<Ledger> Ledgers { get; set; }
    }
}
