using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Payments
{
    public class Payments : Entity
    {
        public Payments(
            string id,
            string name
            ): base(id)
        {
            Name = name;
            
        }
        public string Name { get; set; }
    }
}
