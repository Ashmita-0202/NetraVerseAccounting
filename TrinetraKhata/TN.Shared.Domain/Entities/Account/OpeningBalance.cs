using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Account
{
    public class OpeningBalance : Entity
    {
        public OpeningBalance(
            string id,
            string ledgerId,
            string fiscalYearId,
            decimal balance
            ) : base(id)
        {
            LedgerId = ledgerId;
            FiscalYearId = fiscalYearId;
            Balance = balance;
            
        }
        public string LedgerId { get;set; }
        public virtual Ledger Ledger { get; set; }
        public string FiscalYearId { get; set; }
        public decimal Balance { get; set; }

    }
}
