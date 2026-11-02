using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public class CustomerCategory: Entity
    {
        public CustomerCategory(
            string id,
            string name,
            DateTime createdAt,
            bool isEnabled,
            string customerId,
            bool isActive
            ) : base(id)
        {
            Name = name;
            CreatedAt = createdAt;
            IsEnabled = isEnabled;   
            CustomerId = customerId;
            IsActive = isActive;
        }

        public string Name { get;set; }
        public DateTime CreatedAt { get;set; }
        public bool IsEnabled { get;set; }
        public string CustomerId { get;set; }
        public bool IsActive { get;set; }
        public Customers Customers { get;set; }
    }
}
