using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public class Master : Entity
    {
        public Master(
            string id,
            string name,
            bool isActive
            ): base(id)
        {
            Name = name;
            IsActive = isActive;
            LedgerGroups = new List<LedgerGroup>();
        }
        public string Name {  get; set; }
        public bool IsActive { get; set; }
        public ICollection<LedgerGroup> LedgerGroups { get; set; }
    }
}
