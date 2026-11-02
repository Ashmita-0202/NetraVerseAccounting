using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Account
{
    public class ClosingBalance : Entity
    {
        public ClosingBalance(
            string id,
            string ledgerId,
            string fyId,
            decimal balance,
            string? userId,
            DateTime? createdAt
            ): base(id)
        {
            LedgerId = ledgerId;
            FyId = fyId;
            Balance = balance;
            UserId = userId;
            CreatedAt = createdAt;

            
        }

        public string LedgerId { get; set; }    
        public virtual Ledger Ledger { get; set; }
        public string FyId { get; set;}
        public decimal Balance { get; set; }
        public string? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
