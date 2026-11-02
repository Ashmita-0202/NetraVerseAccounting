using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public class Master : Entity
    {
        public Master(
            string id,
            string name
            ): base(id)
        {
            Name = name;
            LedgerGroups = new List<LedgerGroup>();
        }
        public string Name {  get; set; }
        public ICollection<LedgerGroup> LedgerGroups { get; set; }
    }
}
