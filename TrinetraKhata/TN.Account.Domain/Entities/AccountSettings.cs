using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public sealed class AccountSettings : Entity
    {
        public AccountSettings(
            string id,
            string? key,
            long? value
            ) : base(id)
        {
            Key = key;
            Value = value; 
        }
        public string? Key { get; set; }
        public long? Value { get; set; }
    }
}
